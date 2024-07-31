import Axios from "axios";

const isLocal = true;

const localAPIurl = "http://localhost:44390/";
const liveURL = "https://api.codexroyale.com/";
//const localJSONurl = "C:\Users\evand\OneDrive\Documents\Code\Codex Royale\codexroyale.com\React-Front-End\src\TestJSON";

//const baseURL = isLocal ? localJSONurl : liveURL;

let headers = {};
   
   if (localStorage.user){
   headers.Authorization = `bearer ${localStorage.user['token']}`;
   }
   export const axios = Axios.create({baseURL:localAPIurl, headers,});