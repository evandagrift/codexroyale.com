import { Alert } from 'bootstrap';
import React, { Component, useEffect } from 'react';
import EditableDeck from './EditableDeck';
import TestCard from './TestCard';
import { GetDeckAsync } from "../Utilities/axios-functions";

class Test extends Component {
  constructor(props) {
    super(props);
    this.state = {
      deck: {},
      cards:[
        {
          "Id": 26000000,
          "Name": "Knight",
          "Url": "https://api-assets.clashroyale.com/cards/300/jAj1Q5rclXxU9kVImGqSJxa4wEMfEhvwNQ_4jiGUuqg.png"
        },
        {
          "Id": 26000001,
          "Name": "Archers",
          "Url": "https://api-assets.clashroyale.com/cards/300/W4Hmp8MTSdXANN8KdblbtHwtsbt0o749BbxNqmJYfA8.png"
        },
        {
          "Id": 26000002,
          "Name": "Goblins",
          "Url": "https://api-assets.clashroyale.com/cards/300/X_DQUye_OaS3QN6VC9CPw05Fit7wvSm3XegXIXKP--0.png"
        },
        {
          "Id": 26000003,
          "Name": "Giant",
          "Url": "https://api-assets.clashroyale.com/cards/300/Axr4ox5_b7edmLsoHxBX3vmgijAIibuF6RImTbqLlXE.png"
        },
        {
          "Id": 26000004,
          "Name": "P.E.K.K.A",
          "Url": "https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png"
        },
        {
          "Id": 26000005,
          "Name": "Minions",
          "Url": "https://api-assets.clashroyale.com/cards/300/yHGpoEnmUWPGV_hBbhn-Kk-Bs838OjGzWzJJlQpQKQA.png"
        },
        {
          "Id": 26000006,
          "Name": "Balloon",
          "Url": "https://api-assets.clashroyale.com/cards/300/qBipxLo-3hhCnPrApp2Nn3b2NgrSrvwzWytvREev0CY.png"
        },
        {
          "Id": 26000007,
          "Name": "Witch",
          "Url": "https://api-assets.clashroyale.com/cards/300/cfwk1vzehVyHC-uloEIH6NOI0hOdofCutR5PyhIgO6w.png"
        },
        {
          "Id": 26000008,
          "Name": "Barbarians",
          "Url": "https://api-assets.clashroyale.com/cards/300/TvJsuu2S4yhyk1jVYUAQwdKOnW4U77KuWWOTPOWnwfI.png"
        },
        {
          "Id": 26000009,
          "Name": "Golem",
          "Url": "https://api-assets.clashroyale.com/cards/300/npdmCnET7jmVjJvjJQkFnNSNnDxYHDBigbvIAloFMds.png"
        },
        {
          "Id": 26000010,
          "Name": "Skeletons",
          "Url": "https://api-assets.clashroyale.com/cards/300/oO7iKMU5m0cdxhYPZA3nWQiAUh2yoGgdThLWB1rVSec.png"
        },
        {
          "Id": 26000011,
          "Name": "Valkyrie",
          "Url": "https://cdn.royaleapi.com/static/img/cards-150/valkyrie.png?t=3eaea23cc"
        },
        {
          "Id": 26000012,
          "Name": "Skeleton Army",
          "Url": "https://api-assets.clashroyale.com/cards/300/fAOToOi1pRy7svN2xQS6mDkhQw2pj9m_17FauaNqyl4.png"
        },
        {
          "Id": 26000013,
          "Name": "Bomber",
          "Url": "https://api-assets.clashroyale.com/cards/300/12n1CesxKIcqVYntjxcF36EFA-ONw7Z-DoL0_rQrbdo.png"
        },
        {
          "Id": 26000014,
          "Name": "Musketeer",
          "Url": "https://api-assets.clashroyale.com/cards/300/Tex1C48UTq9FKtAX-3tzG0FJmc9jzncUZG3bb5Vf-Ds.png"
        },
        {
          "Id": 26000015,
          "Name": "Baby Dragon",
          "Url": "https://api-assets.clashroyale.com/cards/300/cjC9n4AvEZJ3urkVh-rwBkJ-aRSsydIMqSAV48hAih0.png"
        },
        {
          "Id": 26000016,
          "Name": "Prince",
          "Url": "https://api-assets.clashroyale.com/cards/300/3JntJV62aY0G1Qh6LIs-ek-0ayeYFY3VItpG7cb9I60.png"
        },
        {
          "Id": 26000017,
          "Name": "Wizard",
          "Url": "https://api-assets.clashroyale.com/cards/300/Mej7vnv4H_3p_8qPs_N6_GKahy6HDr7pU7i9eTHS84U.png"
        },
        {
          "Id": 26000018,
          "Name": "Mini P.E.K.K.A",
          "Url": "https://api-assets.clashroyale.com/cards/300/Fmltc4j3Ve9vO_xhHHPEO3PRP3SmU2oKp2zkZQHRZT4.png"
        },
        {
          "Id": 26000019,
          "Name": "Spear Goblins",
          "Url": "https://api-assets.clashroyale.com/cards/300/FSDFotjaXidI4ku_WFpVCTWS1hKGnFh1sxX0lxM43_E.png"
        },
        {
          "Id": 26000020,
          "Name": "Giant Skeleton",
          "Url": "https://api-assets.clashroyale.com/cards/300/0p0gd0XaVRu1Hb1iSG1hTYbz2AN6aEiZnhaAib5O8Z8.png"
        },
        {
          "Id": 26000021,
          "Name": "Hog Rider",
          "Url": "https://api-assets.clashroyale.com/cards/300/Ubu0oUl8tZkusnkZf8Xv9Vno5IO29Y-jbZ4fhoNJ5oc.png"
        },
        {
          "Id": 26000022,
          "Name": "Minion Horde",
          "Url": "https://api-assets.clashroyale.com/cards/300/Wyjq5l0IXHTkX9Rmpap6HaH08MvjbxFp1xBO9a47YSI.png"
        },
        {
          "Id": 26000023,
          "Name": "Ice Wizard",
          "Url": "https://api-assets.clashroyale.com/cards/300/W3dkw0HTw9n1jB-zbknY2w3wHuyuLxSRIAV5fUT1SEY.png"
        },
        {
          "Id": 26000024,
          "Name": "Royal Giant",
          "Url": "https://api-assets.clashroyale.com/cards/300/mnlRaNtmfpQx2e6mp70sLd0ND-pKPF70Cf87_agEKg4.png"
        },
        {
          "Id": 26000025,
          "Name": "Guards",
          "Url": "https://api-assets.clashroyale.com/cards/300/1ArKfLJxYo6_NU_S9cAeIrfbXqWH0oULVJXedxBXQlU.png"
        },
        {
          "Id": 26000026,
          "Name": "Princess",
          "Url": "https://api-assets.clashroyale.com/cards/300/bAwMcqp9EKVIKH3ZLm_m0MqZFSG72zG-vKxpx8aKoVs.png"
        },
        {
          "Id": 26000027,
          "Name": "Dark Prince",
          "Url": "https://api-assets.clashroyale.com/cards/300/M7fXlrKXHu2IvpSGpk36kXVstslbR08Bbxcy0jQcln8.png"
        },
        {
          "Id": 26000028,
          "Name": "Three Musketeers",
          "Url": "https://api-assets.clashroyale.com/cards/300/_J2GhbkX3vswaFk1wG-dopwiHyNc_YiPhwroiKF3Mek.png"
        },
        {
          "Id": 26000029,
          "Name": "Lava Hound",
          "Url": "https://api-assets.clashroyale.com/cards/300/unicRQ975sBY2oLtfgZbAI56ZvaWz7azj-vXTLxc0r8.png"
        },
        {
          "Id": 26000030,
          "Name": "Ice Spirit",
          "Url": "https://api-assets.clashroyale.com/cards/300/lv1budiafU9XmSdrDkk0NYyqASAFYyZ06CPysXKZXlA.png"
        },
        {
          "Id": 26000031,
          "Name": "Fire Spirit",
          "Url": "https://api-assets.clashroyale.com/cards/300/16-BqusVvynIgYI8_Jci3LDC-r8AI_xaIYLgXqtlmS8.png"
        },
        {
          "Id": 26000032,
          "Name": "Miner",
          "Url": "https://api-assets.clashroyale.com/cards/300/Y4yWvdwBCg2FpAZgs8T09Gy34WOwpLZW-ttL52Ae8NE.png"
        },
        {
          "Id": 26000033,
          "Name": "Sparky",
          "Url": "https://api-assets.clashroyale.com/cards/300/2GKMkBrArZXgQxf2ygFjDs4VvGYPbx8F6Lj_68iVhIM.png"
        },
        {
          "Id": 26000034,
          "Name": "Bowler",
          "Url": "https://api-assets.clashroyale.com/cards/300/SU4qFXmbQXWjvASxVI6z9IJuTYolx4A0MKK90sTIE88.png"
        },
        {
          "Id": 26000035,
          "Name": "Lumberjack",
          "Url": "https://api-assets.clashroyale.com/cards/300/E6RWrnCuk13xMX5OE1EQtLEKTZQV6B78d00y8PlXt6Q.png"
        },
        {
          "Id": 26000036,
          "Name": "Battle Ram",
          "Url": "https://api-assets.clashroyale.com/cards/300/dyc50V2cplKi4H7pq1B3I36pl_sEH5DQrNHboS_dbbM.png"
        },
        {
          "Id": 26000037,
          "Name": "Inferno Dragon",
          "Url": "https://api-assets.clashroyale.com/cards/300/y5HDbKtTbWG6En6TGWU0xoVIGs1-iQpIP4HC-VM7u8A.png"
        },
        {
          "Id": 26000038,
          "Name": "Ice Golem",
          "Url": "https://api-assets.clashroyale.com/cards/300/r05cmpwV1o7i7FHodtZwW3fmjbXCW34IJCsDEV5cZC4.png"
        },
        {
          "Id": 26000039,
          "Name": "Mega Minion",
          "Url": "https://api-assets.clashroyale.com/cards/300/-T_e4YLbuhPBKbYnBwQfXgynNpp5eOIN_0RracYwL9c.png"
        },
        {
          "Id": 26000040,
          "Name": "Dart Goblin",
          "Url": "https://api-assets.clashroyale.com/cards/300/BmpK3bqEAviflqHCdxxnfm-_l3pRPJw3qxHkwS55nCY.png"
        },
        {
          "Id": 26000041,
          "Name": "Goblin Gang",
          "Url": "https://api-assets.clashroyale.com/cards/300/NHflxzVAQT4oAz7eDfdueqpictb5vrWezn1nuqFhE4w.png"
        },
        {
          "Id": 26000042,
          "Name": "Electro Wizard",
          "Url": "https://api-assets.clashroyale.com/cards/300/RsFaHgB3w6vXsTjXdPr3x8l_GbV9TbOUCvIx07prbrQ.png"
        },
        {
          "Id": 26000043,
          "Name": "Elite Barbarians",
          "Url": "https://api-assets.clashroyale.com/cards/300/C88C5JH_F3lLZj6K-tLcMo5DPjrFmvzIb1R2M6xCfTE.png"
        },
        {
          "Id": 26000044,
          "Name": "Hunter",
          "Url": "https://api-assets.clashroyale.com/cards/300/VNabB1WKnYtYRSG7X_FZfnZjQDHTBs9A96OGMFmecrA.png"
        },
        {
          "Id": 26000045,
          "Name": "Executioner",
          "Url": "https://api-assets.clashroyale.com/cards/300/9XL5BP2mqzV8kza6KF8rOxrpCZTyuGLp2l413DTjEoM.png"
        },
        {
          "Id": 26000046,
          "Name": "Bandit",
          "Url": "https://api-assets.clashroyale.com/cards/300/QWDdXMKJNpv0go-HYaWQWP6p8uIOHjqn-zX7G0p3DyM.png"
        },
        {
          "Id": 26000047,
          "Name": "Royal Recruits",
          "Url": "https://api-assets.clashroyale.com/cards/300/jcNyYGUiXXNz3kuz8NBkHNKNREQKraXlb_Ts7rhCIdM.png"
        },
        {
          "Id": 26000048,
          "Name": "Night Witch",
          "Url": "https://api-assets.clashroyale.com/cards/300/NpCrXDEDBBJgNv9QrBAcJmmMFbS7pe3KCY8xJ5VB18A.png"
        },
        {
          "Id": 26000049,
          "Name": "Bats",
          "Url": "https://api-assets.clashroyale.com/cards/300/EnIcvO21hxiNpoI-zO6MDjLmzwPbq8Z4JPo2OKoVUjU.png"
        },
        {
          "Id": 26000050,
          "Name": "Royal Ghost",
          "Url": "https://api-assets.clashroyale.com/cards/300/3En2cz0ISQAaMTHY3hj3rTveFN2kJYq-H4VxvdJNvCM.png"
        },
        {
          "Id": 26000051,
          "Name": "Ram Rider",
          "Url": "https://api-assets.clashroyale.com/cards/300/QaJyerT7f7oMyZ3Fv1glKymtLSvx7YUXisAulxl7zRI.png"
        },
        {
          "Id": 26000052,
          "Name": "Zappies",
          "Url": "https://api-assets.clashroyale.com/cards/300/QZfHRpLRmutZbCr5fpLnTpIp89vLI6NrAwzGZ8tHEc4.png"
        },
        {
          "Id": 26000053,
          "Name": "Rascals",
          "Url": "https://api-assets.clashroyale.com/cards/300/KV48DfwVHKx9XCjzBdk3daT_Eb52Me4VgjVO7WctRc4.png"
        },
        {
          "Id": 26000054,
          "Name": "Cannon Cart",
          "Url": "https://api-assets.clashroyale.com/cards/300/aqwxRz8HXzqlMCO4WMXNA1txynjXTsLinknqsgZLbok.png"
        },
        {
          "Id": 26000055,
          "Name": "Mega Knight",
          "Url": "https://api-assets.clashroyale.com/cards/300/O2NycChSNhn_UK9nqBXUhhC_lILkiANzPuJjtjoz0CE.png"
        },
        {
          "Id": 26000056,
          "Name": "Skeleton Barrel",
          "Url": "https://api-assets.clashroyale.com/cards/300/vCB4DWCcrGbTkarjcOiVz4aNDx6GWLm0yUepg9E1MGo.png"
        },
        {
          "Id": 26000057,
          "Name": "Flying Machine",
          "Url": "https://api-assets.clashroyale.com/cards/300/hzKNE3QwFcrSrDDRuVW3QY_OnrDPijSiIp-PsWgFevE.png"
        },
        {
          "Id": 26000058,
          "Name": "Wall Breakers",
          "Url": "https://api-assets.clashroyale.com/cards/300/_xPphEfC8eEwFNrfU3cMQG9-f5JaLQ31ARCA7l3XtW4.png"
        },
        {
          "Id": 26000059,
          "Name": "Royal Hogs",
          "Url": "https://api-assets.clashroyale.com/cards/300/ASSQJG_MoVq9e81HZzo4bynMnyLNpNJMfSLb3hqydOw.png"
        },
        {
          "Id": 26000060,
          "Name": "Goblin Giant",
          "Url": "https://api-assets.clashroyale.com/cards/300/SoW16cY3jXBwaTDvb39DkqiVsoFVaDWbzf5QBYphJrY.png"
        },
        {
          "Id": 26000061,
          "Name": "Fisherman",
          "Url": "https://api-assets.clashroyale.com/cards/300/U2KZ3g0wyufcuA5P2Xrn3Z3lr1WiJmc5S0IWOZHgizQ.png"
        },
        {
          "Id": 26000062,
          "Name": "Magic Archer",
          "Url": "https://api-assets.clashroyale.com/cards/300/Avli3W7BxU9HQ2SoLiXnBgGx25FoNXUSFm7OcAk68ek.png"
        },
        {
          "Id": 26000063,
          "Name": "Electro Dragon",
          "Url": "https://api-assets.clashroyale.com/cards/300/tN9h6lnMNPCNsx0LMFmvpHgznbDZ1fBRkx-C7UfNmfY.png"
        },
        {
          "Id": 26000064,
          "Name": "Firecracker",
          "Url": "https://api-assets.clashroyale.com/cards/300/c1rL3LO1U2D9-TkeFfAC18gP3AO8ztSwrcHMZplwL2Q.png"
        },
        {
          "Id": 26000065,
          "Name": "Mighty Miner",
          "Url": "https://cdn.royaleapi.com/static/img/cards-150/mighty-miner.png?t=e29b4bcac"
        },
        {
          "Id": 26000066,
          "Name": "Super Witch",
          "Url": "https://api-assets.clashroyale.com/cards/300/Rp4-0PUVW7niVuobP1Lc5jEjVAGgaIEW4-S3ofyKt-g.png"
        },
        {
          "Id": 26000067,
          "Name": "Elixir Golem",
          "Url": "https://api-assets.clashroyale.com/cards/300/puhMsZjCIqy21HW3hYxjrk_xt8NIPyFqjRy-BeLKZwo.png"
        },
        {
          "Id": 26000068,
          "Name": "Battle Healer",
          "Url": "https://api-assets.clashroyale.com/cards/300/KdwXcoigS2Kg-cgA7BJJIANbUJG6SNgjetRQ-MegZ08.png"
        },
        {
          "Id": 26000069,
          "Name": "Skeleton King",
          "Url": "https://api-assets.clashroyale.com/cards/300/dCd69_wN9f8DxwuqOGtR4QgWhHIPIaTNxZ1e23RzAAc.png"
        },
        {
          "Id": 26000070,
          "Name": "Super Lava Hound",
          "Url": "https://cdn.royaleapi.com/static/img/cards/super-lava-hound.png?t=8badf9c0c"
        },
        {
          "Id": 26000072,
          "Name": "Archer Queen",
          "Url": "https://api-assets.clashroyale.com/cards/300/p7OQmOAFTery7zCzlpDdm-LOD1kINTm42AwIHchZfWk.png"
        },
        {
          "Id": 26000073,
          "Name": "Santa Hog Rider",
          "Url": "https://static.wikia.nocookie.net/clashroyale/images/1/1a/SantaHogRiderCard.png"
        },
        {
          "Id": 26000074,
          "Name": "Golden Knight",
          "Url": "https://api-assets.clashroyale.com/cards/300/WJd207D0O1sN-l1FTb8P9KhYL2oF5jY26vRUfTUW3FQ.png"
        },
        {
          "Id": 26000077,
          "Name": "Monk",
          "Url": "https://static.wikia.nocookie.net/clashroyale/images/5/5d/MonkCard.png"
        },
        {
          "Id": 26000080,
          "Name": "Skeleton Dragons",
          "Url": "https://api-assets.clashroyale.com/cards/300/qPOtg9uONh47_NLxGhhFc_ww9PlZ6z3Ry507q1NZUXs.png"
        },
        {
          "Id": 26000082,
          "Name": "Super Mini P.E.K.K.A",
          "Url": "https://static.wikia.nocookie.net/clashroyale/images/b/b3/SuperMiniPEKKACard.png"
        },
        {
          "Id": 26000083,
          "Name": "Mother Witch",
          "Url": "https://api-assets.clashroyale.com/cards/300/fO-Xah8XZkYKaSK9SCp3wnzwxtvIhun9NVY-zzte1Ng.png"
        },
        {
          "Id": 26000084,
          "Name": "Electro Spirit",
          "Url": "https://api-assets.clashroyale.com/cards/300/WKd4-IAFsgPpMo7dDi9sujmYjRhOMEWiE07OUJpvD9g.png"
        },
        {
          "Id": 26000085,
          "Name": "Electro Giant",
          "Url": "https://api-assets.clashroyale.com/cards/300/_uChZkNHAMq6tPb3v6A49xinOe3CnhjstOhG6OZbPYc.png"
        },
        {
          "Id": 26000087,
          "Name": "Phoenix",
          "Url": "https://static.wikia.nocookie.net/clashroyale/images/8/88/PhoenixCard.png"
        },
        {
          "Id": 27000000,
          "Name": "Cannon",
          "Url": "https://api-assets.clashroyale.com/cards/300/nZK1y-beLxO5vnlyUhK6-2zH2NzXJwqykcosqQ1cmZ8.png"
        },
        {
          "Id": 27000001,
          "Name": "Goblin Hut",
          "Url": "https://api-assets.clashroyale.com/cards/300/l8ZdzzNLcwB4u7ihGgxNFQOjCT_njFuAhZr7D6PRF7E.png"
        },
        {
          "Id": 27000002,
          "Name": "Mortar",
          "Url": "https://api-assets.clashroyale.com/cards/300/lPOSw6H7YOHq2miSCrf7ZDL3ANjhJdPPDYOTujdNrVE.png"
        },
        {
          "Id": 27000003,
          "Name": "Inferno Tower",
          "Url": "https://api-assets.clashroyale.com/cards/300/GSHY_wrooMMLET6bG_WJB8redtwx66c4i80ipi4gYOM.png"
        },
        {
          "Id": 27000004,
          "Name": "Bomb Tower",
          "Url": "https://api-assets.clashroyale.com/cards/300/rirYRyHPc97emRjoH-c1O8uZCBzPVnToaGuNGusF3TQ.png"
        },
        {
          "Id": 27000005,
          "Name": "Barbarian Hut",
          "Url": "https://api-assets.clashroyale.com/cards/300/ho0nOG2y3Ch86elHHcocQs8Fv_QNe0cFJ2CijsxABZA.png"
        },
        {
          "Id": 27000006,
          "Name": "Tesla",
          "Url": "https://api-assets.clashroyale.com/cards/300/OiwnGrxFMNiHetYEerE-UZt0L_uYNzFY7qV_CA_OxR4.png"
        },
        {
          "Id": 27000007,
          "Name": "Elixir Collector",
          "Url": "https://api-assets.clashroyale.com/cards/300/BGLo3Grsp81c72EpxLLk-Sofk3VY56zahnUNOv3JcT0.png"
        },
        {
          "Id": 27000008,
          "Name": "X-Bow",
          "Url": "https://api-assets.clashroyale.com/cards/300/zVQ9Hme1hlj9Dc6e1ORl9xWwglcSrP7ejow5mAhLUJc.png"
        },
        {
          "Id": 27000009,
          "Name": "Tombstone",
          "Url": "https://api-assets.clashroyale.com/cards/300/LjSfSbwQfkZuRJY4pVxKspZ-a0iM5KAhU8w-a_N5Z7Y.png"
        },
        {
          "Id": 27000010,
          "Name": "Furnace",
          "Url": "https://api-assets.clashroyale.com/cards/300/iqbDiG7yYRIzvCPXdt9zPb3IvMt7F7Gi4wIPnh2x4aI.png"
        },
        {
          "Id": 27000011,
          "Name": "Barbarian Launcher",
          "Url": "https://static.wikia.nocookie.net/clashroyale/images/d/da/BarbarianLauncherCard.png"
        },
        {
          "Id": 27000012,
          "Name": "Goblin Cage",
          "Url": "https://api-assets.clashroyale.com/cards/300/vD24bBgK4rSq7wx5QEbuqChtPMRFviL_ep76GwQw1yA.png"
        },
        {
          "Id": 27000013,
          "Name": "Goblin Drill",
          "Url": "https://api-assets.clashroyale.com/cards/300/eN2TKUYbih-26yBi0xy5LVFOA0zDftgDqxxnVfdIg1o.png"
        },
        {
          "Id": 28000000,
          "Name": "Fireball",
          "Url": "https://api-assets.clashroyale.com/cards/300/lZD9MILQv7O-P3XBr_xOLS5idwuz3_7Ws9G60U36yhc.png"
        },
        {
          "Id": 28000001,
          "Name": "Arrows",
          "Url": "https://api-assets.clashroyale.com/cards/300/Flsoci-Y6y8ZFVi5uRFTmgkPnCmMyMVrU7YmmuPvSBo.png"
        },
        {
          "Id": 28000002,
          "Name": "Rage",
          "Url": "https://api-assets.clashroyale.com/cards/300/bGP21OOmcpHMJ5ZA79bHVV2D-NzPtDkvBskCNJb7pg0.png"
        },
        {
          "Id": 28000003,
          "Name": "Rocket",
          "Url": "https://api-assets.clashroyale.com/cards/300/Ie07nQNK9CjhKOa4-arFAewi4EroqaA-86Xo7r5tx94.png"
        },
        {
          "Id": 28000004,
          "Name": "Goblin Barrel",
          "Url": "https://api-assets.clashroyale.com/cards/300/CoZdp5PpsTH858l212lAMeJxVJ0zxv9V-f5xC8Bvj5g.png"
        },
        {
          "Id": 28000005,
          "Name": "Freeze",
          "Url": "https://api-assets.clashroyale.com/cards/300/I1M20_Zs_p_BS1NaNIVQjuMJkYI_1-ePtwYZahn0JXQ.png"
        },
        {
          "Id": 28000006,
          "Name": "Mirror",
          "Url": "https://api-assets.clashroyale.com/cards/300/wC6Cm9rKLEOk72zTsukVwxewKIoO4ZcMJun54zCPWvA.png"
        },
        {
          "Id": 28000007,
          "Name": "Lightning",
          "Url": "https://api-assets.clashroyale.com/cards/300/fpnESbYqe5GyZmaVVYe-SEu7tE0Kxh_HZyVigzvLjks.png"
        },
        {
          "Id": 28000008,
          "Name": "Zap",
          "Url": "https://api-assets.clashroyale.com/cards/300/7dxh2-yCBy1x44GrBaL29vjqnEEeJXHEAlsi5g6D1eY.png"
        },
        {
          "Id": 28000009,
          "Name": "Poison",
          "Url": "https://api-assets.clashroyale.com/cards/300/98HDkG2189yOULcVG9jz2QbJKtfuhH21DIrIjkOjxI8.png"
        },
        {
          "Id": 28000010,
          "Name": "Graveyard",
          "Url": "https://api-assets.clashroyale.com/cards/300/Icp8BIyyfBTj1ncCJS7mb82SY7TPV-MAE-J2L2R48DI.png"
        },
        {
          "Id": 28000011,
          "Name": "The Log",
          "Url": "https://api-assets.clashroyale.com/cards/300/_iDwuDLexHPFZ_x4_a0eP-rxCS6vwWgTs6DLauwwoaY.png"
        },
        {
          "Id": 28000012,
          "Name": "Tornado",
          "Url": "https://api-assets.clashroyale.com/cards/300/QJB-QK1QJHdw4hjpAwVSyZBozc2ZWAR9pQ-SMUyKaT0.png"
        },
        {
          "Id": 28000013,
          "Name": "Clone",
          "Url": "https://api-assets.clashroyale.com/cards/300/mHVCet-1TkwWq-pxVIU2ZWY9_2z7Z7wtP25ArEUsP_g.png"
        },
        {
          "Id": 28000014,
          "Name": "Earthquake",
          "Url": "https://api-assets.clashroyale.com/cards/300/XeQXcrUu59C52DslyZVwCnbi4yamID-WxfVZLShgZmE.png"
        },
        {
          "Id": 28000015,
          "Name": "Barbarian Barrel",
          "Url": "https://api-assets.clashroyale.com/cards/300/Gb0G1yNy0i5cIGUHin8uoFWxqntNtRPhY_jeMXg7HnA.png"
        },
        {
          "Id": 28000016,
          "Name": "Heal Spirit",
          "Url": "https://api-assets.clashroyale.com/cards/300/GITl06sa2nGRLPvboyXbGEv5E3I-wAwn1Eqa5esggbc.png"
        },
        {
          "Id": 28000017,
          "Name": "Giant Snowball",
          "Url": "https://api-assets.clashroyale.com/cards/300/7MaJLa6hK9WN2_VIshuh5DIDfGwm0wEv98gXtAxLDPs.png"
        },
        {
          "Id": 28000018,
          "Name": "Royal Delivery",
          "Url": "https://api-assets.clashroyale.com/cards/300/LPg7AGjGI3_xmi7gLLgGC50yKM1jJ2teWkZfoHJcIZo.png"
        }
      ],
      card: {
        "Id": 26000004,
        "Name": "P.E.K.K.A",
        "Url": "https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png"
      }
    };
  }

  async componentDidMount() {
    try {
      let tempDeck = await GetDeckAsync();
      this.setState({ deck: tempDeck });
    }
    catch { }
  }

  render() {
    let draw = '';
    let cards = '';
    const dragEvent = (e,card) => { this.setState({card:card}) }

    const dropEvent = (e, card) => {
      console.log(card);
      
      console.log(e);
      let tempDeck = this.state.deck;
      let x = 1;
      let tempCards = this.state.cards;
      let index = tempCards.findIndex(c => c.Id == card.Id)

      switch (card.Id) {
        case tempDeck.Card1.Id:
          this.setState({card:tempDeck.Card1})
          tempDeck.Card1 = card;
          break;
        case tempDeck.Card2.Id:
          this.setState({card:tempDeck.Card2})
          tempDeck.Card2 = card;
          break;
        case tempDeck.Card3.Id:
          this.setState({card:tempDeck.Card3})
          tempDeck.Card3 = card;
          break;
        case tempDeck.Card4.Id:
          this.setState({card:tempDeck.Card4})
          tempDeck.Card4 = card;
          break;
        case tempDeck.Card5.Id:
          this.setState({card:tempDeck.Card5})
          tempDeck.Card5 = card;
          break;
        case tempDeck.Card6.Id:
          this.setState({card:tempDeck.Card6})
          tempDeck.Card6 = card;
          break;
        case tempDeck.Card7.Id:
          this.setState({card:tempDeck.Card7})
          tempDeck.Card7 = card;
          break;
        case tempDeck.Card8.Id:
          this.setState({card:tempDeck.Card8})
          tempDeck.Card8 = card;
          break;
      }
      this.setState({ deck: tempDeck })

    }

    const handleDragOver = e => { e.preventDefault(); }

    if (this.state.deck) {
      draw = (<div>{<EditableDeck dragEvent={dragEvent} dragOver={handleDragOver} dropEvent={dropEvent} deck={this.state.deck} />}{<TestCard dragOver={handleDragOver} key={"$card-" + this.state.card.Id} dragEvent={dragEvent} dropEvent={dropEvent} card={this.state.card} />}</div>);
      cards = this.state.cards.map((b, i) => (
        <TestCard key={"$card-" + i} dragOver={handleDragOver} dragEvent={dragEvent} dropEvent={dropEvent} card={b} />
      ));
    }
    else {
      return (<h1>Loading</h1>)
    }

    return (<div>
      {draw}
      {cards}
    </div>
    );
  }

}
export default Test;  