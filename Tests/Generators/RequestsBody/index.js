import getKeysGenerator from "./getKeysGenerator.js";
import postKeysGenerator from "./postKeysGenerator.js";
import postPaymentsGenerator from "./postPaymentsGenerator.js";
import postConciliationsGenerator from "./postConciliationsGenerator.js";

async function createRequestsBody() {
    try {
        console.log("\n*****************************************\n");
        console.log("Creating requests body data...");
        await getKeysGenerator()
        await postKeysGenerator()
        await postPaymentsGenerator()
        await postConciliationsGenerator();
        console.log("\n*****************************************\n");
    } catch (error) {
        console.error("An error occurred during generate seed data:", error);
    }
}

await createRequestsBody();
