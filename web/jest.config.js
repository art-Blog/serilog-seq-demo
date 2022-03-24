module.exports = {
    roots: ["<rootDir>/Src"],
    testMatch: [
        "<rootDir>/Src/**/__tests__/**/*.{vue,js}",
        "<rootDir>/Src/**/*.{spec,test}.{vue,js}",
    ],
    testEnvironment: "jsdom",
    transform: {
        "^.+\\.(vue)$": "<rootDir>/node_modules/vue3-jest",
        "^.+\\.(js)$": "<rootDir>/node_modules/babel-jest",
    },
    transformIgnorePatterns: [
        "<rootDir>/node_modules/",
        "[/\\\\]node_modules[/\\\\].+\\.(js|jsx|mjs|cjs|ts|tsx)$",
        "^.+\\.module\\.(css|sass|scss|less)$",
    ],
    moduleNameMapper: {
        "^~Src(.*)$": "<rootDir>/Src$1",
        "^~Components(.*)$": "<rootDir>/Src/Components$1",
        "^~Containers(.*)$": "<rootDir>/Src/Containers$1",
        "^~Utils(.*)$": "<rootDir>/Src/Utils$1",
        "^~Vuex(.*)$": "<rootDir>/Src/Vuex$1",
        "\\.(jpg|jpeg|png)$": "<rootDir>/Src/TestConfig/fileMock.js",
        "\\.(css|scss|svg)$": "identity-obj-proxy"
    },
    moduleFileExtensions: ["vue", "js", "json", "node"],
    resetMocks: true,
};
