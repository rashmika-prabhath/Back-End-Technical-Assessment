// src/App.tsx
import React, { useEffect, useState } from "react";
import axios from "axios";
import "./App.css";

type Cell = "empty" | "hit" | "miss";
type Grid = Cell[][];

const App: React.FC = () => {
    const [grid, setGrid] = useState<Grid>([]);
    const [message, setMessage] = useState<string>("");

    useEffect(() => {
        startGame();
    }, []);

    const startGame = async () => {
        try {
            const response = await axios.post("https://localhost:7139/api/game/start");
            setGrid(response.data.board);
            setMessage(response.data.message);
        } catch (error) {
            console.error("Error starting game", error);
        }
    };

    const handleCellClick = async (row: number, col: number) => {
        try {
            const response = await axios.post("https://localhost:7139/api/game/attack", {
                row,
                col,
            });
            setMessage(response.data.message);
            updateGrid(row, col, response.data.status);
        } catch (error) {
            console.error("Error attacking", error);
        }
    };

    const updateGrid = (row: number, col: number, status: string) => {
        setGrid((prevGrid) => {
            const newGrid = [...prevGrid];
            newGrid[row][col] = status === "hit" ? "hit" : "miss";
            return newGrid;
        });
    };

    return (
        <div className="App">
            <h1>Battleship Game</h1>
            <div className="grid">
                {grid.map((row, rowIndex) =>
                    row.map((cell, colIndex) => (
                        <div
                            key={`${rowIndex}-${colIndex}`}
                            className={`cell ${cell}`}
                            onClick={() => handleCellClick(rowIndex, colIndex)}
                        >
                            {cell === "hit" ? "X" : cell === "miss" ? "O" : ""}
                        </div>
                    ))
                )}
            </div>
            <p>{message}</p>
        </div>
    );
};

export default App;
