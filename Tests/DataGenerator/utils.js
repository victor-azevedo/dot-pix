import fs from "fs";

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