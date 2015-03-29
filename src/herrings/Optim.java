package herrings;

import java.util.ArrayList;
import java.util.List;

public class Optim {

    int i, k, l, a, b, j, h, g, v, u, m, sum, p, ind, max, x, y, z;

    Spolka spol = new Spolka(5);

    List newzbior = new ArrayList();

    public void funkcja() {

        int[][] tab = new int[90][3];
        tab = spol.czesc();


        List zbior = new ArrayList();
        zbior = spol.zbior();

        List suma = new ArrayList();
        List indeks = new ArrayList();
        v = tab.length * tab.length;
        h = v * v;
        int wyn[][][][] = new int[h][tab.length + 1][2][tab.length + 1];
        int wyn1[] = new int[tab.length + 1];

        int wyn2[][] = new int[h][tab.length + 1];
        int wyn3[] = new int[tab.length + 1];


        for (i = 0; i < tab.length; i++) {
            System.out.println(tab[i][0] + "    " + tab[i][1] + "     " + tab[i][2]);
        }
        for (i = 0; i < tab.length; i++) {
            wyn[i][0][0][0] = i + 1;
            wyn[i][1][0][0] = tab[i][0];
            wyn[i][0][1][0] = tab[i][1];
            wyn[i][0][0][1] = tab[i][2];
            wyn2[i][tab.length] = tab[i][0];

        }


        for (i = 0; i < h; i++) {
            ind = 0;
            sum = 0;
            for (p = 1; p < wyn[1].length; p++) {
                if (wyn[i][p][0][0] != 0) {
                    ind = ind + 1;
                    sum = sum + wyn[i][p][0][0];
                }
            }
            if (ind == tab.length) {
                break;
            }

            for (k = 0; k < tab.length; k++) {
                for (j = 1; j < wyn[1].length; j++) {
                    if (wyn[i][j][0][0] != 0) {
                        if (wyn[i][j][0][0] != tab[k][0] && wyn[i][0][0][j] != tab[k][0] && wyn[i][j][0][0] != tab[k][2]) {
                            a = v * ind + sum + tab[k][0];
                        }

                        for (b = 0; b < wyn[1].length; b++) {
                            wyn[a][b][0][0] = wyn[i][b][0][0];
                            wyn[a][0][0][b] = wyn[i][0][0][b];
                        }
                        wyn[a][0][0][0] = a;
                        wyn[a][ind + 1][0][0] = tab[k][0];
                        wyn[a][0][0][ind + 1] = tab[k][2];
                        wyn[a][0][1][0] = wyn[i][0][1][0] + tab[k][1];
                    }
                }
            }


            for (u = 0; u < h; u++) {
                for (j = 1; j < wyn[1].length; j++) {
                    for (p = 1; p < wyn[1].length; p++) {
                        if (wyn[u][j][0][0] == wyn[u][p][0][0] && wyn[u][j][0][0] != 0 && p != j) {

                            for (b = 0; b < wyn[1].length; b++) {

                                wyn[u][b][0][0] = 0;
                                wyn[u][0][0][b] = 0;
                                wyn[u][0][1][0] = 0;

                            }
                        }
                    }
                }
            }
        }


        for (i = 0; i < h; i++) {
            if (wyn[i][0][0][0] != 0) {
                System.out.println("Zbior " + wyn[i][0][0][0]);
                for (p = 1; p < wyn[1].length; p++) {
                    System.out.println("Pracownik " + wyn[i][p][0][0] + " suma " + wyn[i][0][1][0] + " sledzi " + wyn[i][0][0][p]);
                }
            }
        }


        max = 0;
        for (i = 0; i < h; i++) {

            if (wyn[i][0][1][0] > max) {
                max = wyn[i][0][1][0];
            }
        }
        System.out.println(max);

    }
}