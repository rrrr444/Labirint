using Xunit;
using Labirint;


namespace LabirintTest.Tests
{
    public class MazeTests
    {
        private int[,] layout = new int[,]
        {
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 2 },
            { 1, 1, 1, 1, 1 }
        };

        private Maze maze;

        public MazeTests()
        {
            maze = new Maze(layout);
        }

        [Fact]
        public void IsWall_ReturnsTrue_ForWall()
        {
            Xunit.Assert.True(maze.IsWall(0, 0)); // Стена
            Xunit.Assert.True(maze.IsWall(1, 0)); // Стена
        }

        [Fact]
        public void IsWall_ReturnsFalse_ForPath()
        {
            Xunit.Assert.False(maze.IsWall(1, 1)); // Путь
            Xunit.Assert.False(maze.IsWall(1, 2)); // Путь
        }

        [Fact]
        public void IsExit_ReturnsTrue_ForExit()
        {
            Xunit.Assert.True(maze.IsExit(1, 4)); // Выход
        }

        [Fact]
        public void IsExit_ReturnsFalse_ForNonExit()
        {
            Xunit.Assert.False(maze.IsExit(1, 1)); // Не выход
        }
    }
}
