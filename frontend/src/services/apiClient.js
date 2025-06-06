import axios from 'axios';

// Create an axios instance specifically for your ASP.NET Core backend
const apiClient = axios.create({
  baseURL: 'https://localhost:7252',
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  },
  // For development only - helps with CORS issues
  withCredentials: false
});

// Add request interceptor to handle requests properly
apiClient.interceptors.request.use(
  config => {
    // Log outgoing requests for debugging
    console.log(`API Request: ${config.method.toUpperCase()} ${config.url}`);
    return config;
  },
  error => {
    console.error('API Request Error:', error);
    return Promise.reject(error);
  }
);

// Add response interceptor for error handling
apiClient.interceptors.response.use(
  response => {
    // Log successful responses for debugging
    console.log(`API Response: ${response.status} ${response.config.url}`);
    return response;
  },
  error => {
    // Handle 404 errors specifically
    if (error.response && error.response.status === 404) {
      console.error(`404 Error: Resource not found at ${error.config.url}`);
    }
    
    console.error('API Error Details:', {
      message: error.message,
      status: error.response?.status,
      data: error.response?.data,
      url: error.config?.url
    });
    return Promise.reject(error);
  }
);

export default apiClient;
