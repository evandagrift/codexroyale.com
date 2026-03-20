import Axios from "axios";
const localURL = "http://localhost:44390/";
const baseURL = "https://api.codexroyale.com/";
let headers = {};
if (localStorage.user){
   headers.Authorization = `bearer ${localStorage.user['token']}`;
}
export const axios = Axios.create({baseURL:localURL,
headers,
});
