import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import resultService from '../../services/resultService';
import assessmentService from '../../services/assessmentService';
import courseService from '../../services/courseService';
import authService from '../../services/authService';

const ResultList = ({ userOnly = false }) => {
  const [results, setResults] = useState([]);
  const [assessments, setAssessments] = useState({});
  const [courses, setCourses] = useState({});
  const [users, setUsers] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const currentUser = authService.getCurrentUser();

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch all results
        let resultData = await resultService.getAllResults();
        
        // If userOnly flag is true, filter for current user's results only
        if (userOnly && currentUser) {
          resultData = resultData.filter(r => r.userId === currentUser.userId);
        }
        
        setResults(resultData);
        
        // Fetch all assessments to get their titles
        const assessmentData = await assessmentService.getAllAssessments();
        const assessmentMap = {};
        const uniqueCourseIds = new Set();
        
        assessmentData.forEach(assessment => {
          assessmentMap[assessment.assessmentId] = assessment;
          if (assessment.courseId) {
            uniqueCourseIds.add(assessment.courseId);
          }
        });
        
        setAssessments(assessmentMap);
        
        // Fetch courses for the assessments
        const courseIds = Array.from(uniqueCourseIds);
        const courseMap = {};
        
        for (const courseId of courseIds) {
          try {
            const course = await courseService.getCourseById(courseId);
            courseMap[courseId] = course;
          } catch (err) {
            console.error(`Failed to load course ${courseId}:`, err);
          }
        }
        
        setCourses(courseMap);
        
        // For instructor view, we also need user names
        if (!userOnly && currentUser?.role === 'Instructor') {
          // In a real application, you'd fetch user data here
          // Since we don't have a getAllUsers endpoint in this example, we'll skip it
        }
      } catch (err) {
        setError('Failed to load results. Please try again later.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [userOnly, currentUser]);

  const formatDate = (dateString) => {
    const options = { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  if (loading) {
    return (
      <div className="d-flex justify-content-center my-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger m-3" role="alert">
        {error}
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2>{userOnly ? 'My Results' : 'All Assessment Results'}</h2>
      
      {results.length === 0 ? (
        <div className="alert alert-info mt-3">
          No results found.
        </div>
      ) : (
        <div className="table-responsive mt-3">
          <table className="table table-striped table-hover">
            <thead className="table-primary">
              <tr>
                <th>Assessment</th>
                <th>Course</th>
                {!userOnly && <th>Student</th>}
                <th>Score</th>
                <th>Date</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {results.map((result) => {
                const assessment = assessments[result.assessmentId];
                const course = assessment ? courses[assessment.courseId] : null;
                const passThreshold = 0.7; // 70% is passing
                const scorePercentage = assessment ? (result.score / assessment.maxScore) : 0;
                const isPassed = scorePercentage >= passThreshold;
                
                return (
                  <tr key={result.resultId}>
                    <td>
                      {assessment ? (
                        <Link to={`/assessments/${assessment.assessmentId}`}>
                          {assessment.title}
                        </Link>
                      ) : (
                        'Unknown Assessment'
                      )}
                    </td>
                    <td>
                      {course ? (
                        <Link to={`/courses/${course.courseId}`}>
                          {course.title}
                        </Link>
                      ) : (
                        'Unknown Course'
                      )}
                    </td>
                    {!userOnly && <td>{currentUser?.name || 'Unknown User'}</td>}
                    <td>
                      {assessment ? (
                        <span>{result.score} / {assessment.maxScore}</span>
                      ) : (
                        result.score
                      )}
                    </td>
                    <td>{formatDate(result.attemptDate)}</td>
                    <td>
                      <span className={`badge ${isPassed ? 'bg-success' : 'bg-danger'}`}>
                        {isPassed ? 'Passed' : 'Failed'}
                      </span>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ResultList;
