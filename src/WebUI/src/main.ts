const sleep = (ms: number) => {
    return new Promise((resolve, reject) => setTimeout(resolve, ms));
  };


async function mainLoop() {
    while (true) {
        console.log("Hello")
        await sleep(2000);
    }
}
mainLoop();