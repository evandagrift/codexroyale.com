import { axios } from "../axios";
import { FormatTag } from "./scripts";

export async function LoginFunctionAsync(username, password) {
  try {
    const response = await axios.post("Users/login", {
      Username: username,
      Password: password,
    });
    return response.data;
  } catch {
    return undefined;
  }
}

export async function GetBattlesAsync(paginationInfo) {
  try {
    const response = await axios.get(`battles?pageIndex=${paginationInfo.pageIndex}&itemsPerPage=${paginationInfo.itemsPerPage}`);
    return response;
  } catch(error){
    console.error("Error fetching battles:", error);
    return null;
  }
}
export async function GetPlayerTagAsync(id) {
  try {
    const response = await axios.get("teams/playertag/" + id);
    return response.data;
  } catch {
    return undefined;
  }
}

export async function GetClanAsync(tag) {
  try {
    const searchTag = FormatTag(tag);
    const response = await axios.get("clans/" + searchTag);
    return response.data;
  } catch {
    return undefined;
  }
}


export async function GetPlayerBattlesAsync(playerTag, paginationInfo) {
  if (playerTag) {
    try {
      const searchTag = FormatTag(playerTag);

      const responseBattles = await axios.get("battles/player/" + searchTag);
      return responseBattles.data
    } catch {
      return undefined;
    }
  }
}

export async function GetChestsAsync(playerTag) {
  try {
    const response = await axios.get(
      "player/" + FormatTag(playerTag) + "/chests");
    return response.data;
  } catch {
    return undefined;
  }
}

export async function GetTopDecks(playerTag) {
  console.log("Getting Player's Top Decks by player tag")
  try {
    const response = await axios.get("player/" + FormatTag(playerTag) + "/decks");
    return response.data;
  } catch {
    return undefined;
  }
}

export async function updateUserSettings(user, tag, password, newPassword) {
  const config = {
    headers: { Authorization: `bearer ${user["bearerToken"]}` },
  };
  try {
    const response = await axios.post(
      "Users/Update",
      {
        user: {
          Username: user.username,
          Password: password,
          Tag: tag,
          BearerToken: user.bearerToken,
        },
        newPassword: newPassword,
      },
      config
    );

    return response.data;
  } catch {
    return undefined;
  }
}

export async function getPlayerDataAsync(tag) {
  try {
    if(tag != undefined){
    const response = await axios.get("players/" + FormatTag(tag));
    return response.data;
    }
  } catch {
    return undefined;
  }
}

export async function getTeamDataAsync(id) {
  try {
    const response = await axios.get("players/id/" + id);
    return response.data;
  } catch {
    return undefined;
  }
}

export async function getAllCards() {
  try {
    const response = await axios.get("cards");
    return response.data;
  } catch {
    return undefined;
  }
}

// export async function GetDeckAsync(tag) {
//   try {
//     const response = await axios.get("decks/" + 6224);
//     return response.data;
//   } catch {
//     return undefined;
//   }
// }

/*
  async function getCard(id) {
    const response = axios.get("cards/" + id, config);
    return response.data;
  }

  */

// const getBattles = async (e) => {
//     let fetchedBattles = await GetBattlesAsync();
//     if(fetchedBattles != null)
//     {
//         setBattles(fetchedBattles);

//     setBattlesFetched(true);
//     }
// }