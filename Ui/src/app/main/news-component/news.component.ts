import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Title} from "@angular/platform-browser";
import {environment} from "../../../environments/environment";
import {News} from "../../common/models/news";


@Component({
    selector: "news-page",
    templateUrl: 'news.component.html',
    styleUrls: ["news.component.css"]
})
export class NewsComponent implements OnInit {
    announcements: Array<News> = new Array<News>();

    constructor(private titleService: Title, private httpClient: HttpClient) {

    }

    ngOnInit(): void {
        this.titleService.setTitle("News");
        this.httpClient.get(environment.apiUrl + '/api/data/news')
            .subscribe((data: Array<News>) => {
                this.announcements = data;
            });
    }
}
