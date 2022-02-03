import Axios from "axios";
const localURL = "//localhost:52003/api/"
let headers = {};
if (localStorage.user){
   headers.Authorization = `bearer ${localStorage.user['token']}`;
}
export const axios = Axios.create({baseURL:localURL,
headers,
});
