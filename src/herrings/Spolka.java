package herrings;

import java.util.Random;
import java.util.ArrayList;
import java.util.List;

public class Spolka {

    Random gen = new Random();

    int n, i, j;

    public Spolka(int n) {

        this.n = n;
    }

    public int[][] czesc() {

        int[][] prac = new int[n][3];

        for (i = 0; i < n; i++) {

            prac[i][0] = i + 1;
            prac[i][1] = gen.nextInt(10) + 1;
        }

        prac[0][2] = 1;

        for (i = 1; i < n; i++) {

            prac[i][2] = gen.nextInt(n) + 1;

            if (prac[i][2] == i + 1) {
                while (prac[i][2] == i + 1) {
                    prac[i][2] = gen.nextInt(n) + 1;
                }
            }
        }
        return prac;
    }


    public List zbior() {

        int[][] tab = new int[100][100];
        tab = czesc();
        List zbior = new ArrayList();
        int w;
        for (w = 0; w < tab.length; w++) {
            zbior.add(tab[w][0]);
        }

        return zbior;

    }

    public int silnia(int n) {
        int wy = 1;
        for (i = 1; i < n + 1; i++) {
            wy = wy * i;
        }
        return wy;

    }

}
