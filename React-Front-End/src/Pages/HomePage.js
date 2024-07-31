import React, { useContext, useEffect, useState } from "react";
import { UserContext } from "../UserContext";
import BattleCollection from "../components/BattleCollection";
import SearchBox from "../components/SearchBox";
import ChestCollection from "../components/ChestCollection";
import styles from "../cssModules/HomePage.module.css";
import { GetBattlesAsync } from "../Utilities/axios-functions";

const HomePage = () => {
  const { user } = useContext(UserContext);

  const [chests, setChests] = useState(undefined);
  const [loading, setLoading] = useState(false);
  const [battles, setBattles] = useState([]);
  const [paginationInfo, setPaginationInfo] = useState({
    pageIndex: 1,
    itemsPerPage: 10,
    totalPages: 1,
    hasPreviousPage: false,
    hasNextPage: false
  });

  const fetchData = async () => {
    if (loading) return; // Prevent multiple concurrent fetches
    setLoading(true);
    try {
      const response = await GetBattlesAsync(paginationInfo);
      if (response && response.status === 200) {
        setBattles((prevBattles) => [...prevBattles, ...response.data.Items]);
        setPaginationInfo((prevState) => ({
          ...prevState,
          totalPages: response.data.PaginationInfo.TotalPages,
          hasPreviousPage: response.data.PaginationInfo.HasPreviousPage,
          hasNextPage: response.data.PaginationInfo.HasNextPage
        }));
      }
    } catch (error) {
      console.error("Error fetching HomePage data:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [paginationInfo.pageIndex]);

  useEffect(() => {
    const handleScroll = () => {
      const { scrollTop, clientHeight, scrollHeight } = document.documentElement;
      if (scrollTop + clientHeight >= scrollHeight - 20 && !loading && paginationInfo.pageIndex < paginationInfo.totalPages) {
        setPaginationInfo((prevState) => ({
          ...prevState,
          pageIndex: prevState.pageIndex + 1
        }));
      }
    };

    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, [loading, paginationInfo.pageIndex, paginationInfo.totalPages]);

  let upcomingChests = user ? (
    <div className={styles.chestCollection}>
      <ChestCollection playerTag={user.tag} />
    </div>
  ) : undefined;

  let greeting = user ? (
    <div className={styles.greeting}>
      <h1>Welcome {user.username}</h1>
    </div>
  ) : undefined;

  let loadingIcon = loading? (
    <div className={styles.loadingIcon}>
      <img src={require("../assets/icons8-loading.gif")} />
    </div>) : undefined;

  return (
    <div className={styles.homePage}>
      <img
        className={styles.homeImgTemp}
        src={require("../assets/KeyArt_Season_011.png")}
        alt="Season 11" />
      <div className={styles.overlayTemp}>
        {greeting}
        {upcomingChests}
        <SearchBox />
      </div>
      <div className={styles.battleCollection}>
        <BattleCollection battles={battles} />
        {loadingIcon}
      </div>
    </div>
  );
};

export default HomePage;
