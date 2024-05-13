import { CubeEvent } from "../../Enums/CubeEvent.ts";


export function generateScramble(event: CubeEvent, length: number) {
    const scramble: string[] = [];
    let randomMove = "";
    const moves: string[] = ["U", "D", "R", "L", "F", "B"];
    const movesOneByOne: string[] = ["x", "y", "z"];
    const movesPyraminx: string[] = ["L", "U", "R", "B"];
    switch (event) {
        case CubeEvent.OneByOne:
            for (let i = 0; i < length; i++) {
                randomMove = movesOneByOne[Math.floor(Math.random() * movesOneByOne.length)];
                while ((scramble.length >= 1 && scramble[i - 1] === randomMove)) {
                    randomMove = movesOneByOne[Math.floor(Math.random() * movesOneByOne.length)];
                }
                scramble.push(randomMove);
            }
            for (let i = 0; i < scramble.length; i++) {
                if (Math.random() < 0.33) {
                    scramble[i] += "";
                } else if (Math.random() < 0.66) {
                    scramble[i] += "'";
                } else {
                    scramble[i] += "2";
                }
            }
            break;
        case CubeEvent.TwoByTwo:
            for (let i = 0; i < length; i++) {
                randomMove = moves[Math.floor(Math.random() * moves.length)];
                while ((scramble.length >= 1 && scramble[i - 1] === randomMove) || (scramble.length >= 2 && scramble[i - 2] === randomMove)) {
                    randomMove = moves[Math.floor(Math.random() * moves.length)];
                }
                scramble.push(randomMove);
            }
            for (let i = 0; i < scramble.length; i++) {
                if (Math.random() < 0.33) {
                    scramble[i] += "";
                } else if (Math.random() < 0.66) {
                    scramble[i] += "'";
                } else {
                    scramble[i] += "2";
                }
            }
            break;
        case CubeEvent.ThreeByThree:
            for (let i = 0; i < length; i++) {
                randomMove = moves[Math.floor(Math.random() * moves.length)];
                while ((scramble.length >= 1 && scramble[i - 1] === randomMove) || (scramble.length >= 2 && scramble[i - 2] === randomMove)) {
                    randomMove = moves[Math.floor(Math.random() * moves.length)];
                }
                scramble.push(randomMove);
            }
            for (let i = 0; i < scramble.length; i++) {
                if (Math.random() < 0.33) {
                    scramble[i] += "";
                } else if (Math.random() < 0.66) {
                    scramble[i] += "'";
                } else {
                    scramble[i] += "2";
                }
            }
            break;
        case CubeEvent.FourByFour:
            for (let i = 0; i < length; i++) {
                randomMove = moves[Math.floor(Math.random() * moves.length)];
                while ((scramble.length >= 1 && scramble[i - 1] === randomMove) || (scramble.length >= 2 && scramble[i - 2] === randomMove) || (scramble.length >= 3 && scramble[i - 3] === randomMove)) {
                    randomMove = moves[Math.floor(Math.random() * moves.length)];
                }
                scramble.push(randomMove);
            }
            for (let i = 0; i < scramble.length; i++) {
                if (Math.random() < 0.33) {
                    scramble[i] += "w";
                }
                if (Math.random() < 0.33) {
                    scramble[i] += "";
                } else if (Math.random() < 0.66) {
                    scramble[i] += "'";
                } else {
                    scramble[i] += "2";
                }
            }
            break;
        case CubeEvent.FiveByFive:
            for (let i = 0; i < length; i++) {
                randomMove = moves[Math.floor(Math.random() * moves.length)];
                while ((scramble.length >= 1 && scramble[i - 1] === randomMove) || (scramble.length >= 2 && scramble[i - 2] === randomMove) || (scramble.length >= 3 && scramble[i - 3] === randomMove) || (scramble.length >= 4 && scramble[i - 4] === randomMove)) {
                    randomMove = moves[Math.floor(Math.random() * moves.length)];
                }
                scramble.push(randomMove);
            }
            for (let i = 0; i < scramble.length; i++) {
                if (Math.random() < 0.5) {
                    scramble[i] = scramble[i].toLowerCase();
                }
                if (Math.random() < 0.33) {
                    scramble[i] += "";
                } else if (Math.random() < 0.66) {
                    scramble[i] += "'";
                } else {
                    scramble[i] += "2";
                }
            }
            break;
        case CubeEvent.Pyraminx:
            for (let i = 0; i < length; i++) {
                randomMove = movesPyraminx[Math.floor(Math.random() * movesPyraminx.length)];
                while ((scramble.length >= 1 && scramble[i - 1] === randomMove)) {
                    randomMove = movesPyraminx[Math.floor(Math.random() * movesPyraminx.length)];
                }
                scramble.push(randomMove);
            }
            for (let i = 0; i < scramble.length; i++) {
                if (Math.random() < 0.50) {
                    scramble[i] = scramble[i].toLowerCase();
                }
                if (Math.random() < 0.50) {
                    scramble[i] += "";
                } else {
                    scramble[i] += "'";
                }
            }
            break;
    }


    return scramble.join(" ");
}
