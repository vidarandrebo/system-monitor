import {ModuleRecord} from "./dataRecords";
import {Module} from "./module";
import {httpGetWithBody} from "./httpMethods";
import {Device} from "./device";
import {Value} from "./value";

const sleep = (ms: number) => {
    return new Promise((resolve, reject) => setTimeout(resolve, ms));
};

async function main() {
    let modules = await GetModules();
    await mainLoop(modules);
}
async function GetModules() : Promise<Map<string, Module>> {
    let modules = new Map<string, Module>();
    let data = await httpGetWithBody<ModuleRecord[]>("/Data");
    let modulesRecords = data.body;
    let tableBody = document.getElementById("temp-table-body")
    for (let i = 0; i < modulesRecords.length; i++) {
        let module = new Module(modulesRecords[i].name, tableBody);
        let devices = modulesRecords[i].devices;
        for (let j = 0; j < devices.length; j++) {
            let device = new Device(devices[j].name, tableBody);
            device.value = new Value(devices[j].value, device.mainElement);
            module.devices.set(devices[j].deviceId, device);
        }
        modules.set(data.body[i].moduleId, module);
    }
    return modules;
}

async function mainLoop(modules: Map<string, Module>) {
    while (true) {
        let data = await httpGetWithBody<ModuleRecord[]>("/Data");
        let modulesRecords = data.body;
        for (let i = 0; i < modulesRecords.length; i++) {
            let moduleRecord = modulesRecords[i];
            for (let j = 0; j < moduleRecord.devices.length; j++) {
                let deviceId = moduleRecord.devices[j].deviceId;
                modules
                    .get(moduleRecord.moduleId)
                    .devices
                    .get(deviceId)
                    .value
                    .update(moduleRecord.devices[j].value);
            }
        }
        await sleep(2000);
    }
}

main().then().catch().finally();