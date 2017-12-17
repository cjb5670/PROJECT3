using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreCounter {

    private static int score;

	public static int Score
    {
        get { return score; }
        set { score = value; }
    }
}
