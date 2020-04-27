import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Title} from "@angular/platform-browser";
import {Link, LinkType} from "../../common/models/link";
import {Person} from "../../common/models/person";
import {environment} from "../../../environments/environment";
import {Project} from "../../common/models/project";
import {Publication} from "../../common/models/publication";

@Component({
    selector: "info-page",
    templateUrl: 'info.component.html',
    styleUrls: ["info.component.css"]
})
export class InfoComponent implements OnInit {

    people: Array<Person> = Array<Person>();
    publications: Array<Publication> = Array<Publication>();
    projects: Array<Project> = Array<Project>();
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
        this.httpClient.get(environment.apiUrl + '/api/data/publications')
            .subscribe((publications: Array<Publication>) => {
                if (publications != null && publications.length != 0) {
                    this.publications = publications;
                }
            });
        this.httpClient.get(environment.apiUrl + '/api/data/projects')
            .subscribe((projects: Array<Project>) => {
                if (projects != null && projects.length != 0) {
                    this.projects = projects;
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

    downloadPublication(publication) {
        window.location.href = environment.apiUrl + "/api/data/publications?publicationId=" + publication.id;
    }

}
