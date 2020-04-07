import {Component, OnInit} from '@angular/core';
import {Title} from "@angular/platform-browser";
import {AboutInfo} from "../../common/models/about-info";
import {Link, LinkType} from "../../common/models/link";
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "../../../environments/environment";


@Component({
    selector: "home-page",
    templateUrl: 'home-page.component.html',
    styleUrls: ["home-page.component.css"]
})
export class HomePageComponent implements OnInit {
    aboutInfo: AboutInfo = new AboutInfo();
    generalLinks: Array<Link> = Array<Link>();

    constructor(private titleService: Title, private httpClient: HttpClient) {
        this.httpClient.get(environment.apiUrl + '/api/data/info')
            .subscribe((aboutInfo: AboutInfo) => {
                if (aboutInfo != null) {
                    this.aboutInfo = aboutInfo;
                }
            });
        const params = new HttpParams().set('type', LinkType.GENERAL.toString());
        this.httpClient.get(environment.apiUrl + '/api/data/links', {params})
            .subscribe((links: Array<Link>) => {
                if (links != null && links.length != 0) {
                    this.generalLinks = links;
                }
            });
    }

    ngOnInit(): void {
        this.titleService.setTitle('Home');
    }
}