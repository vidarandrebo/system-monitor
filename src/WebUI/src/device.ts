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
        this.nameElement.innerText = this.name;
        this.mainElement.appendChild(this.nameElement);
        this.mainElement.style.display = 'none';
        table.appendChild(this.mainElement);
    }
    public updateHidden() {
        if (this.value.isNonZero()) {
            this.mainElement.style.display = 'table-row';
        }
    }
}
