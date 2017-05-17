using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester1
{
    public class MeshGridContainer
    {
        public int[][] container1;
        public int[][] container2;
        public int container1_row;
        public int container1_col;
        public int container2_row;
        public int container2_col;

        public MeshGridContainer() { }
        public MeshGridContainer(int c1_row, int c1_col, int c2_row, int c2_col)
        {
            this.container1_row = c1_row;
            this.container1_col = c1_col;
            this.container2_row = c2_row;
            this.container2_col = c2_col;
            container1 = new int[container1_row][];
            for (int i = 0; i < container1_row; i++)
            {
                container1[i] = new int[container1_col];
            }
            container2 = new int[container2_row][];
            for (int i = 0; i < container2_row; i++)
            {
                container2[i] = new int[container2_col];
            }
        }
    }
}
