export type ModuleRecord = {
    moduleId: string;
    name: string;
    devices: DeviceRecord[];
}

export type DeviceRecord = {
    deviceId: string;
    name:string;
    value: ValueRecord;
}

export type ValueRecord = {
    current: string;
    min: string;
    max: string;
    average: string;
}