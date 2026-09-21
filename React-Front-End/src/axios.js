import Axios from "axios";
import { getMockResponse } from "./mocks/axios-mock";

const localURL = "https://localhost:52715/";
const hostedURL = "https://api.codexroyale.com/";
let headers = {};
if (localStorage.user){
   headers.Authorization = `bearer ${localStorage.user['token']}`;
}

const useMocks = process.env.REACT_APP_USE_MOCKS === 'false';
console.log('[Axios] REACT_APP_USE_MOCKS =', process.env.REACT_APP_USE_MOCKS);
console.log('[Axios] useMocks =', useMocks);

const mockAdapter = async (config) => {
  console.log('[AxiosMock] Intercepting', config.method?.toUpperCase(), config.url);
  try {
    const response = await getMockResponse(config);
    console.log('[AxiosMock] Returned mock response for', config.method?.toUpperCase(), config.url, 'status=', response.status);
    return response;
  } catch (error) {
    console.error('[AxiosMock] Error:', error);
    throw error;
  }
};

export const axios = Axios.create({
  baseURL: hostedURL,
  headers,
  adapter: useMocks ? mockAdapter : undefined,
});
