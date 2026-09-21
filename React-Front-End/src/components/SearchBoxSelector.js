import React from "react";

const SearchBoxSelector = ({ onSelectPlayer, onSelectClan, searching }) => {
  const playerClass = searching === "player" ? "searchTabActive" : "searchTab";
  const clanClass = searching === "clan" ? "searchTabActive" : "searchTab";

  return (
    <div className="searchTabs">
      <button type="button" className={playerClass} onClick={onSelectPlayer}>
        Player
      </button>
      <button type="button" className={clanClass} onClick={onSelectClan}>
        Clan
      </button>
    </div>
  );
};

export default SearchBoxSelector;
