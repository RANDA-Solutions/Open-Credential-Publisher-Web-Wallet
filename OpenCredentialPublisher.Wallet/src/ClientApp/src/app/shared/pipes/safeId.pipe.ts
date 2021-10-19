// https://gist.github.com/adamrecsko/0f28f474eca63e0279455476cc11eca7
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({ name: 'safeId' })
export class SafeIdPipe implements PipeTransform {
    constructor(public sanitizer: DomSanitizer) {
    }
    transform(text: string): SafeHtml {
        if (text) {
            let x = text.replace(new RegExp(':', 'g'), '');
            x = x.replace(new RegExp('/', 'g'), '');
            x = x.replace(new RegExp('.', 'g'), '');
            return x;
        } else {
            return text;
        }
    }
}
