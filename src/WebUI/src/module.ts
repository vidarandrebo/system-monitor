import { Device } from "./device";
export class Module {
    name: string;
    devices: Map<string, Device>;
    nameElement: HTMLElement;
    constructor(name:string, table: HTMLElement) {
        this.name = name;
        let row = document.createElement("tr");
        this.nameElement = document.createElement("td");
        this.nameElement.innerHTML = "<b>" + name + "</b>";
        row.appendChild(this.nameElement);
        table.appendChild(row);
        this.devices = new Map<string, Device>();
    }
}