import fs from "fs";

export const SEED_LENGTH = 500_000;

export function saveDataToJson(data, filepath) {
    fs.writeFile(filepath, JSON.stringify(data), "utf-8",
        (err) => {
            if (err)
                console.log(err);
            else {
                console.log(`Data successfully created at ${filepath}!`);
            }
        });
}