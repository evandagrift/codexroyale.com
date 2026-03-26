import React, { useContext, useEffect, useState, useRef, useCallback } from "react";
import { UserContext } from "../UserContext";
import BattleCollection from "../components/BattleCollection";
import SearchBox from "../components/SearchBox";
import ChestCollection from "../components/ChestCollection";
import styles from "../cssModules/HomePage.module.css";
import { GetBattlesAsync } from "../Utilities/axios-functions";

const HomePage = () => {
  const { user } = useContext(UserContext);

  const [chests, setChests] = useState(undefined);
  // const [loading, setLoading] = useState(false);
  // const [battles, setBattles] = useState([]);
  // const [paginationInfo, setPaginationInfo] = useState({
  //   pageIndex: 1,
  //   itemsPerPage: 10,
  //   totalPages: 1,
  //   hasPreviousPage: false,
  //   hasNextPage: false
  // });
const loading = useRef(false);

const battles = useRef([]);
const paginationInfo = useRef({
    pageIndex: 1,
    itemsPerPage: 10,
    totalPages: 1,
    hasPreviousPage: false,
    hasNextPage: false
  });

  const fetchData = useCallback(async () => {
    if (loading.current) return;
    loading.current = true;
    try {
      const response = await GetBattlesAsync(paginationInfo.current);
      if (response && response.status === 200) {
        battles.current = [...battles.current, ...response.data.Items];
        paginationInfo.current = {
          ...paginationInfo.current,
          totalPages: response.data.PaginationInfo.TotalPages,
          hasPreviousPage: response.data.PaginationInfo.HasPreviousPage,
          hasNextPage: response.data.PaginationInfo.HasNextPage
        };
      }
    } catch (error) {
      console.error("Error fetching HomePage data:", error);
    } finally {
      loading.current = false;
    }
  });

  useEffect(() => {
    console.log("Home Page constructor useEffect ")
    fetchData();
  }, []);

  useEffect(() => {
    const handleScroll = () => {
      const { scrollTop, clientHeight, scrollHeight } = document.documentElement;
      if (!loading.current && scrollTop + clientHeight >= scrollHeight - 20 && paginationInfo.current.pageIndex < paginationInfo.current.totalPages) {
        paginationInfo.current = {
          ...paginationInfo.current,
          pageIndex: paginationInfo.current.pageIndex + 1
        };
      console.log("Scroll triggered")
      }
      console.log("Handle Scroll Function")
    };
    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, [loading.current, battles.current, paginationInfo.current]);

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

  let battleCollectionDisplayed = battles.current.length > 0?
      <div className={styles.battleCollection}>
        <BattleCollection battles={battles.current} />
        {loadingIcon}
      </div> :
      <div></div>;


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
        <h2>Recently Recorded Battles</h2>
        {battleCollectionDisplayed}
        {loadingIcon}
      </div>
    </div>
  );
};

export default HomePage;