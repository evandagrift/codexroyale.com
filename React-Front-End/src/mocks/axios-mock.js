const mockDelay = () => new Promise(r => setTimeout(r, 400));

export async function getMockResponse(config) {
  await mockDelay();

  const url = config.url || '';
  const method = (config.method || 'get').toLowerCase();

  console.log('[AxiosMock]', method.toUpperCase(), url);

  if (method === 'post' && url.includes('Users/login')) {
    return {
      data: {
        username: 'testuser',
        tag: '%2390ABCDEF',
        token: 'mock-bearer-token-123',
        bearerToken: 'mock-bearer-token-123'
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (method === 'post' && url.includes('Users/Update')) {
    return {
      data: { success: true, message: 'User settings updated' },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (method === 'post' && url.includes('Users/ResetPassword')) {
    return {
      data: { success: true, message: 'Password reset email sent' },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('battles?pageIndex=') || url === 'battles') {
    return {
      data: {
        Items: [
          {
            Team1Name: 'MockTeamA',
            Team2Name: 'MockTeamB',
            Team1Crowns: 3,
            Team2Crowns: 1,
            Team1StartingTrophies: 5000,
            Team2StartingTrophies: 4900,
            Team1TrophyChange: 30,
            Team2TrophyChange: -25,
            Team1Id: 'mock-team-1',
            Team2Id: 'mock-team-2',
            Team1DeckA: [
              { Id: 26000004, Name: 'P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png', Level: 13 },
              { Id: 26000030, Name: 'Hog Rider', Url: 'https://api-assets.clashroyale.com/cards/300/9ed9b2d7e7b7d9c8b8b8b8b8b8b8b8b.png', Level: 13 },
              { Id: 26000003, Name: 'Musketeer', Url: 'https://api-assets.clashroyale.com/cards/300/URJJYXEczyrfS2hlZnKEA1XkcF-VJq3ajO6xt_P5w5E.png', Level: 13 },
              { Id: 26000006, Name: 'Fireball', Url: 'https://api-assets.clashroyale.com/cards/300/lcRX8cQDFFr_ATt5cCEEg5jUjfNZVS8eWdCXj0dqC4Y.png', Level: 13 },
              { Id: 26000029, Name: 'Valkyrie', Url: 'https://api-assets.clashroyale.com/cards/300/0l0YZeGiTCAnJq-6aD8_7rVUv8f7u9k8j6h5g4f3d2s1.png', Level: 13 },
              { Id: 26000011, Name: 'Wizard', Url: 'https://api-assets.clashroyale.com/cards/300/oS-NSAVZPyVh4l38t3V4f1-1q2w3e4r5t6y7u8i9o0p.png', Level: 13 },
              { Id: 26000021, Name: 'Mini P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/pIX3qMn4r2S4XqVnV5Xr2Qr1Qz2Ws3Xr4Yz5Wa6Vb7Xc.png', Level: 13 },
              { Id: 26000014, Name: 'Mega Minion', Url: 'https://api-assets.clashroyale.com/cards/300/8f1I0vPqRzTs2Wr4Yz6Ab8Cd0Ef2Gh4Ij6Kl8Mn0Op.png', Level: 13 }
            ],
            Team2DeckA: [
              { Id: 26000004, Name: 'P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png', Level: 12 },
              { Id: 26000003, Name: 'Musketeer', Url: 'https://api-assets.clashroyale.com/cards/300/URJJYXEczyrfS2hlZnKEA1XkcF-VJq3ajO6xt_P5w5E.png', Level: 12 },
              { Id: 26000030, Name: 'Hog Rider', Url: 'https://api-assets.clashroyale.com/cards/300/9ed9b2d7e7b7d9c8b8b8b8b8b8b8b8b.png', Level: 12 },
              { Id: 26000006, Name: 'Fireball', Url: 'https://api-assets.clashroyale.com/cards/300/lcRX8cQDFFr_ATt5cCEEg5jUjfNZVS8eWdCXj0dqC4Y.png', Level: 12 },
              { Id: 26000029, Name: 'Valkyrie', Url: 'https://api-assets.clashroyale.com/cards/300/0l0YZeGiTCAnJq-6aD8_7rVUv8f7u9k8j6h5g4f3d2s1.png', Level: 12 },
              { Id: 26000011, Name: 'Wizard', Url: 'https://api-assets.clashroyale.com/cards/300/oS-NSAVZPyVh4l38t3V4f1-1q2w3e4r5t6y7u8i9o0p.png', Level: 12 },
              { Id: 26000021, Name: 'Mini P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/pIX3qMn4r2S4XqVnV5Xr2Qr1Qz2Ws3Xr4Yz5Wa6Vb7Xc.png', Level: 12 },
              { Id: 26000014, Name: 'Mega Minion', Url: 'https://api-assets.clashroyale.com/cards/300/8f1I0vPqRzTs2Wr4Yz6Ab8Cd0Ef2Gh4Ij6Kl8Mn0Op.png', Level: 12 }
            ],
            BattleTime: '20240904120000'
          },
          {
            Team1Name: 'MockTeamC',
            Team2Name: 'MockTeamD',
            Team1Crowns: 0,
            Team2Crowns: 3,
            Team1StartingTrophies: 4800,
            Team2StartingTrophies: 5100,
            Team1TrophyChange: -28,
            Team2TrophyChange: 32,
            Team1Id: 'mock-team-3',
            Team2Id: 'mock-team-4',
            Team1DeckA: [
              { Id: 26000004, Name: 'P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png', Level: 11 },
              { Id: 26000003, Name: 'Musketeer', Url: 'https://api-assets.clashroyale.com/cards/300/URJJYXEczyrfS2hlZnKEA1XkcF-VJq3ajO6xt_P5w5E.png', Level: 11 }
            ],
            Team2DeckA: [
              { Id: 26000004, Name: 'P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png', Level: 12 },
              { Id: 26000003, Name: 'Musketeer', Url: 'https://api-assets.clashroyale.com/cards/300/URJJYXEczyrfS2hlZnKEA1XkcF-VJq3ajO6xt_P5w5E.png', Level: 12 }
            ],
            BattleTime: '20240903180000'
          }
        ],
        PaginationInfo: {
          TotalPages: 3,
          HasPreviousPage: false,
          HasNextPage: true
        }
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('teams/playertag/')) {
    return {
      data: '%2390ABCDEF',
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('clans/') || url.includes('Clans/')) {
    return {
      data: {
        Name: 'Mock Clan',
        Tag: '%23ABC123',
        Description: 'A test clan for offline development',
        Members: 45,
        RequiredTrophies: 3000,
        DonationsPerWeek: 5000,
        ClanWarTrophies: 1000,
        ClanChestLevel: 5,
        ClanChestStatus: 'Inactive',
        Type: 'Open',
        LocationCode: 'US',
        BadgeId: 100,
        ClanScore: 45000,
        MemberList: [
          {
            Name: 'PlayerOne',
            Tag: '%2390ABCDEF',
            Role: 'Leader',
            Trophies: 6000,
            Donations: 500,
            DonationsReceived: 300
          },
          {
            Name: 'PlayerTwo',
            Tag: '%2390DEF123',
            Role: 'Co-Leader',
            Trophies: 5500,
            Donations: 200,
            DonationsReceived: 150
          }
        ]
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('battles/player/')) {
    return {
      data: [
        {
          Team1Name: 'PlayerA',
          Team2Name: 'PlayerB',
          Team1Crowns: 3,
          Team2Crowns: 0,
          Team1StartingTrophies: 5000,
          Team2StartingTrophies: 4800,
          Team1TrophyChange: 30,
          Team2TrophyChange: -28,
          Team1Id: 'mock-p1',
          Team2Id: 'mock-p2',
          Team1DeckA: [
            { Id: 26000004, Name: 'P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png', Level: 13 }
          ],
          Team2DeckA: [
            { Id: 26000003, Name: 'Musketeer', Url: 'https://api-assets.clashroyale.com/cards/300/URJJYXEczyrfS2hlZnKEA1XkcF-VJq3ajO6xt_P5w5E.png', Level: 12 }
          ],
          BattleTime: '20240904120000'
        }
      ],
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('player/') && url.includes('/chests')) {
    return {
      data: [
        { Index: 0, Name: 'Silver Chest', Image: 'https://api-assets.clashroyale.com/chests/silver.png' },
        { Index: 1, Name: 'Gold Chest', Image: 'https://api-assets.clashroyale.com/chests/gold.png' },
        { Index: 2, Name: 'Giant Chest', Image: 'https://api-assets.clashroyale.com/chests/giant.png' },
        { Index: 3, Name: 'Magical Chest', Image: 'https://api-assets.clashroyale.com/chests/magical.png' },
        { Index: 4, Name: 'Legendary Chest', Image: 'https://api-assets.clashroyale.com/chests/legendary.png' },
        { Index: 5, Name: 'Epic Chest', Image: 'https://api-assets.clashroyale.com/chests/epic.png' }
      ],
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('player/') && url.includes('/decks')) {
    return {
      data: [
        {
          Card1Id: 26000004,
          Card2Id: 26000030,
          Card3Id: 26000003,
          Card4Id: 26000006,
          Card5Id: 26000029,
          Card6Id: 26000011,
          Card7Id: 26000021,
          Card8Id: 26000014,
          Wins: 10,
          Loss: 2,
          WinLossRate: '83.3%'
        },
        {
          Card1Id: 26000003,
          Card2Id: 26000004,
          Card3Id: 26000006,
          Card4Id: 26000030,
          Card5Id: 26000011,
          Card6Id: 26000029,
          Card7Id: 26000021,
          Card8Id: 26000014,
          Wins: 8,
          Loss: 3,
          WinLossRate: '72.7%'
        }
      ],
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('players/') && !url.includes('battles')) {
    return {
      data: {
        Name: 'MockPlayer',
        ExpLevel: 13,
        Tag: '%2390ABCDEF',
        Deck: [
          { Id: 26000004, Name: 'P.E.K.K.A', Level: 13 },
          { Id: 26000030, Name: 'Hog Rider', Level: 13 },
          { Id: 26000003, Name: 'Musketeer', Level: 13 },
          { Id: 26000006, Name: 'Fireball', Level: 13 },
          { Id: 26000029, Name: 'Valkyrie', Level: 13 },
          { Id: 26000011, Name: 'Wizard', Level: 13 },
          { Id: 26000021, Name: 'Mini P.E.K.K.A', Level: 13 },
          { Id: 26000014, Name: 'Mega Minion', Level: 13 }
        ],
        Trophies: 6000,
        BestTrophies: 6500,
        Wins: 5000,
        Losses: 2000,
        ThreeCrownWins: 500,
        StarPoints: 5000,
        CardsDiscovered: 90,
        BattleCount: 7000,
        LastSeen: '20240904120000',
        Clan: {
          Name: 'Mock Clan',
          Tag: '%23ABC123',
          Role: 'Leader',
          Donations: 500,
          DonationsReceived: 300,
          TotalDonations: 10000,
          ClanCardsCollected: 5000,
          WarDayWins: 20
        },
        ClanTag: '%23ABC123'
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('cards')) {
    return {
      data: [
        { Id: 26000004, Name: 'P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/MlArURKhn_zWAZY-Xj1qIRKLVKquarG25BXDjUQajNs.png', Level: 13, MaxLevel: 13 },
        { Id: 26000003, Name: 'Musketeer', Url: 'https://api-assets.clashroyale.com/cards/300/URJJYXEczyrfS2hlZnKEA1XkcF-VJq3ajO6xt_P5w5E.png', Level: 13, MaxLevel: 13 },
        { Id: 26000030, Name: 'Hog Rider', Url: 'https://api-assets.clashroyale.com/cards/300/9ed9b2d7e7b7d9c8b8b8b8b8b8b8b8b.png', Level: 13, MaxLevel: 13 },
        { Id: 26000006, Name: 'Fireball', Url: 'https://api-assets.clashroyale.com/cards/300/lcRX8cQDFFr_ATt5cCEEg5jUjfNZVS8eWdCXj0dqC4Y.png', Level: 13, MaxLevel: 13 },
        { Id: 26000029, Name: 'Valkyrie', Url: 'https://api-assets.clashroyale.com/cards/300/0l0YZeGiTCAnJq-6aD8_7rVUv8f7u9k8j6h5g4f3d2s1.png', Level: 13, MaxLevel: 13 },
        { Id: 26000011, Name: 'Wizard', Url: 'https://api-assets.clashroyale.com/cards/300/oS-NSAVZPyVh4l38t3V4f1-1q2w3e4r5t6y7u8i9o0p.png', Level: 13, MaxLevel: 13 },
        { Id: 26000021, Name: 'Mini P.E.K.K.A', Url: 'https://api-assets.clashroyale.com/cards/300/pIX3qMn4r2S4XqVnV5Xr2Qr1Qz2Ws3Xr4Yz5Wa6Vb7Xc.png', Level: 13, MaxLevel: 13 },
        { Id: 26000014, Name: 'Mega Minion', Url: 'https://api-assets.clashroyale.com/cards/300/8f1I0vPqRzTs2Wr4Yz6Ab8Cd0Ef2Gh4Ij6Kl8Mn0Op.png', Level: 13, MaxLevel: 13 }
      ],
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('Decks/') || url.includes('decks')) {
    return {
      data: {
        Card1: { Id: 26000004, Name: 'P.E.K.K.A', Level: 13 },
        Card2: { Id: 26000030, Name: 'Hog Rider', Level: 13 },
        Card3: { Id: 26000003, Name: 'Musketeer', Level: 13 },
        Card4: { Id: 26000006, Name: 'Fireball', Level: 13 },
        Card5: { Id: 26000029, Name: 'Valkyrie', Level: 13 },
        Card6: { Id: 26000011, Name: 'Wizard', Level: 13 },
        Card7: { Id: 26000021, Name: 'Mini P.E.K.K.A', Level: 13 },
        Card8: { Id: 26000014, Name: 'Mega Minion', Level: 13 }
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  if (url.includes('playersnapshot/')) {
    return {
      data: {
        Name: 'MockPlayer',
        Tag: '%2390ABCDEF',
        Deck: []
      },
      status: 200,
      statusText: 'OK',
      headers: {},
      config,
      request: {}
    };
  }

  console.warn('[AxiosMock] No mock matched for', method.toUpperCase(), url);
  return {
    data: { message: 'Mock not configured for this endpoint' },
    status: 404,
    statusText: 'Not Found',
    headers: {},
    config,
    request: {}
  };
}
