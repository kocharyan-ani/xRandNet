import {Component, OnInit} from '@angular/core';
import {Location} from "@angular/common";

@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.css']
})
export class ApplicationComponent implements OnInit {

    constructor(private location: Location) {
    }

    ngOnInit() {
    }

    public back() {
        this.location.back()
    }
}
