import { Value } from "./value";

export class Device {
    name: string;
    value: Value;
    mainElement: HTMLElement;
    nameElement: HTMLElement;
    constructor(name: string, table: HTMLElement) {
        this.name = name;
        this.mainElement = document.createElement("tr")
        this.nameElement = document.createElement("td")
        this.nameElement.innerText = name;
        this.mainElement.appendChild(this.nameElement);
        table.appendChild(this.mainElement);
    }
}