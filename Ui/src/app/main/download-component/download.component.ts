import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Title} from "@angular/platform-browser";
import {environment} from "../../../environments/environment";
import {Software} from "../../common/models/software";


@Component({
    selector: "download-page",
    templateUrl: 'download.component.html',
    styleUrls: ["download.component.css"]
})
export class DownloadComponent implements OnInit {

    software: Array<Software> = [];
    private _selectedSoftware: Software;
    public apiUrl: string = environment.apiUrl;

    constructor(private httpClient: HttpClient,
                private titleService: Title) {

    }

    ngOnInit(): void {
        this.titleService.setTitle("Downloads");
        this.httpClient.get(environment.apiUrl + '/api/app/versions')
            .subscribe((data: Array<object>) => {
                for (let obj of data) {
                    this.software.push(new Software(obj["version"], obj["releaseNotes"], obj["releaseDate"].substring(0, 10)));
                }
                this.software.sort((a, b) =>
                    a.version.localeCompare(b.version)
                ).reverse();
                this.software[0].label = this.software[0].version + " (latest)";
                this.selectedSoftware = this.software[0];
            });
    }

    download(selectedSoftware) {
        console.log(selectedSoftware.version);
        window.location.href = environment.apiUrl + "/api/app?version=" + selectedSoftware.version;
    }

    get selectedSoftware(): Software {
        return this._selectedSoftware;
    }

    set selectedSoftware(value: Software) {
        this._selectedSoftware = value;
    }


}
