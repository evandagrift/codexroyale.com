import Axios from "axios";
const localURL = "https://localhost:52715/";
const hostedURL = "https://api.codexroyale.com/";
let headers = {};
if (localStorage.user){
   headers.Authorization = `bearer ${localStorage.user['token']}`;
}
export const axios = Axios.create({baseURL:localURL,
headers,
});
