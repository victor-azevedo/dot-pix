import {faker} from "@faker-js/faker";
import {UniqueEnforcer} from "enforce-unique";
import {saveDataToJson, SEED_LENGTH, SEED_FILE_PATH} from "../utils.js"

const dataLength = SEED_LENGTH;
const filepath = `${SEED_FILE_PATH}/users.json`

function createRandomUsers() {
    const uniqueEnforcerCpf = new UniqueEnforcer();

    function createUser() {
        const cpf = uniqueEnforcerCpf.enforce(() => {
            return faker.string.numeric(11);
        });
        const name = faker.person.fullName();
        return {
            "Name": name,
            "Cpf": cpf,
        };
    }

    const users = []
    for (let i = 0; i < dataLength; i++) {
        users.push(createUser());
    }
    return users;
}

export default async function createUsersJson() {
    try {
        const data = createRandomUsers();

        await saveDataToJson(data, filepath);
    } catch
        (error) {
        console.error(`Error creating data: ${error}`);
    }
}