import { axios } from "../axios";


export function FormatTag(tag)
{
  
  let tagFormat = '';
  let formattedTag = '';
  if(tag)
{
  tagFormat = tag.substring(0,3);
  if(tagFormat == "%23")
  {
    formattedTag = tag;
  }
  else if(tagFormat[0] == '#')
  {
    formattedTag = "%23"+tag.substring(1);
  }
  else  
  {
    formattedTag = "%23" + tag;
  }
}
return formattedTag.toUpperCase();
}
export function ConvertTimer(dateTime)
{
  if(dateTime.length > 0){

  //2021 07 17 T 06 42 25
  let year = dateTime.substring(0,4);
  let month = dateTime.substring(4,6);
  let day = dateTime.substring(6,8);  
  let hours = dateTime.substring(9,11);
  let minutes = dateTime.substring(11,13);
  let seconds = dateTime.substring(14,15);
  //- (offset/60)

var convertedDate = new Date(year, month-1, day, hours, minutes, seconds,0);

let returnString = convertedDate.toDateString();

let timeString = convertedDate.toTimeString().substring(0,8) + " UTC";

returnString = returnString + ", " + timeString;
   return returnString;
  }
  else return null
}

export async function GetClanAsync(tag){
  if(tag){
    let modifiedTag = FormatTag(tag);
  return axios.get('Clans/'+tag)
.then((response) => response.data
, (error) => { 
});
}
}


export async function RequestResetPasswordPostAsync(userEmail){
  return axios.post('Users/ResetPassword/'+userEmail)
.then((response) => response.data
, (error) => { 
});

}


export async function ResetPasswordPostAsync(password, passwordResetCode){
  return axios.post('Users/ResetPassword/Authenticate',{
    password: password,
    passwordResetCode: passwordResetCode
  })
.then((response) => response.data
, (error) => { 
});

}


export function GetDeckById(deckId){
  const response = axios.get("Decks/"+deckId).then();
  return response.data;
}
/*

    const config = { headers: {} };
    axios
    .post("Users/login", {
      Username: username,
      Password: password,
    })
    .then(
      (response) => {
        
        user = response.data;
      },
      (error) => {
        user = null;
      }
    );
    */