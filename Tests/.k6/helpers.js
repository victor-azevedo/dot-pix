import {SharedArray} from "k6/data";

export const SEED_TEST_FILE_PATH = "../../Database/seedTest.json"

export const API_URL = "http://localhost:5200";

export const defaultOptions = {
    vus: 10,
    duration: "10s"
}

export const usersSeed = new SharedArray('users', function () {
    const seed = JSON.parse(open(SEED_TEST_FILE_PATH));
    return seed["Users"];
});

export const paymentProvidersSeed = new SharedArray('paymentProviders', function () {
    const seed = JSON.parse(open(SEED_TEST_FILE_PATH));
    return seed["PaymentProviders"];
});

export function parseJsonToArray(dataName, filepath) {
    return new SharedArray(dataName, function () {
        return JSON.parse(open(filepath));
    });
}

export function getRandomToken() {
    const paymentProviders = paymentProvidersSeed
    return paymentProviders[Math.floor((Math.random() * paymentProviders.length))]["Token"]
}