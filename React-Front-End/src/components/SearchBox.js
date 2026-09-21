import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FormatTag } from "../Utilities/scripts";
import SearchBoxSelector from "./SearchBoxSelector";

const SearchBox = () => {
  const [searching, setSearching] = useState("player");
  const [searchInput, setSearchInput] = useState("");
  const navigate = useNavigate();

  const handleSearch = (e) => {
    e.preventDefault();
    const formattedTag = FormatTag(searchInput.trim());
    if (!formattedTag) return;

    if (searching === "player") {
      navigate(`/Player/${formattedTag}`);
    } else {
      navigate(`/Clan/${formattedTag}`);
    }
  };

  return (
    <div className="searchCard">
      <h2 className="searchTitle">Search For Player or Clan</h2>
      <SearchBoxSelector
        onSelectPlayer={() => setSearching("player")}
        onSelectClan={() => setSearching("clan")}
        searching={searching}
      />
      <form className="searchForm" onSubmit={handleSearch}>
        <input
          className="searchInput"
          placeholder={searching === "player" ? "#29PGJURQL" : "#8CYPL8R"}
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />
        <button className="searchButton" type="submit">
          Get {searching} Data
        </button>
      </form>
    </div>
  );
};

export default SearchBox;
