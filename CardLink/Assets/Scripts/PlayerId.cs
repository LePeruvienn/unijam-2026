using UnityEngine;

public enum PlayerId{ P1 = 0, P2 = 1 }

public static class PlayerExtension
{
    public static PlayerId OtherPlayer(this PlayerId playerId) 
    {
        return playerId == PlayerId.P1 ? PlayerId.P2 : PlayerId.P1;
    }

    public static string PlayerName(this PlayerId playerId)
    {
        return playerId == PlayerId.P1 ? "P1" : "P2";
    }
}
