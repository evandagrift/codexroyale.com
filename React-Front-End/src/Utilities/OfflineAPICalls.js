import battles from "C:\Users\evand\OneDrive\Documents\Code\Codex Royale\codexroyale.com\React-Front-End\src\TestJSON\battles.txt";

export async function getBattles() {
    try {
      const response = await axios.get("playersnapshot/" + FormatTag(tag));
      return response.data;
    } catch {
      return undefined;
    }
  }