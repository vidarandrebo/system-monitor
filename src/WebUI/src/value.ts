import {ValueRecord} from "./dataRecords";

export class Value {
    current: string;
    currentElement: HTMLElement;
    min: string;
    minElement: HTMLElement;
    max: string;
    maxElement: HTMLElement;
    average: string;
    averageElement: HTMLElement;
    constructor(values: ValueRecord, deviceElement: HTMLElement) {
        Object.assign(this, values);
        this.currentElement = document.createElement("td")
        this.minElement = document.createElement("td")
        this.maxElement = document.createElement("td")
        this.averageElement = document.createElement("td")
        this.currentElement.innerText = this.current;
        this.minElement.innerText = this.min;
        this.maxElement.innerText = this.max;
        this.averageElement.innerText = this.average;
        deviceElement.appendChild(this.currentElement);
        deviceElement.appendChild(this.minElement);
        deviceElement.appendChild(this.maxElement);
        deviceElement.appendChild(this.averageElement);
    }
    public update(values: ValueRecord) {
        Object.assign(this, values);
        this.currentElement.innerText = this.current;
        this.minElement.innerText = this.min;
        this.maxElement.innerText = this.max;
        this.averageElement.innerText = this.average;
    }
}