import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Title} from "@angular/platform-browser";
import {Link, LinkType} from "../../common/models/link";
import {Person} from "../../common/models/person";
import {environment} from "../../../environments/environment";

@Component({
    selector: "info-page",
    templateUrl: 'info.component.html',
    styleUrls: ["info.component.css"]
})
export class InfoComponent implements OnInit {

    people: Array<Person> = Array<Person>();
    literatureLinks: Array<Link> = Array<Link>();


    constructor(private titleService: Title, private httpClient: HttpClient) {

    }

    ngOnInit(): void {
        this.titleService.setTitle('Info');
        this.httpClient.get(environment.apiUrl + '/api/data/people')
            .subscribe((people: Array<Person>) => {
                if (people != null && people.length != 0) {
                    this.people = people;
                }
            });
        const params = new HttpParams().set('type', LinkType.LITERATURE.toString());
        this.httpClient.get(environment.apiUrl + '/api/data/links', {params})
            .subscribe((links: Array<Link>) => {
                if (links != null && links.length != 0) {
                    this.literatureLinks = links;
                }
            });
    }

    getImageUrl(person) {
        if (person.imageUrl == null || person.imageUrl == '') {
            return 'https://shorturl.at/kqRS6'
        }
        return person.imageUrl
    }
}
