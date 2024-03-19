import * as dotenv from "dotenv";
import fs from "fs";

dotenv.config();

export const SEED_LENGTH = process.env.SEED_LENGTH || 100;
export const BODY_TEST_LENGTH = process.env.BODY_TEST_LENGTH || 100;
export const SEED_FILE_PATH = "./SeedData";
export const BODY_TEST_FILE_PATH = "./RequestsData";

export async function saveDataToJson(data, filepath) {
    console.log(`Creating data ${filepath}...`);

    fs.writeFile(filepath, JSON.stringify(data), "utf-8", (err) => {
        if (err) console.log(err);
        else {
            console.log(`Data successfully created: ${filepath}!`);
        }
    });
}

export function readDataFromJson(filepath) {
    console.log(`Reading data from ${filepath}...`);

    try {
        const data = fs.readFileSync(filepath, "utf-8");
        console.log(`Data successfully read from ${filepath}!`);
        return JSON.parse(data);
    } catch (err) {
        console.log(err);
        return null;
    }
}

export function addDateToData(data) {
    const dateNow = new Date().toISOString();
    return data.map((element) => ({
        ...element,
        created_at: dateNow,
        updated_at: dateNow,
    }));
}
