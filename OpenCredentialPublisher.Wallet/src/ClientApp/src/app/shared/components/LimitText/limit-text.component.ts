import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewEncapsulation } from '@angular/core';


@Component({
  selector: 'app-limit-text',
  templateUrl: 'limit-text.component.html',
  encapsulation: ViewEncapsulation.Emulated
})

export class LimitTextComponent implements OnInit, OnChanges {
  @Input() text = '';
  @Input() limit = 20;

  newText = '';
  cleanText = '';
  isMore = false;
  constructor() {
  }

  ngOnInit() {
  }

  setText() {
    if (!this.text) {
      this.newText = '';
      this.isMore = false;
      return;
    }
    let shortenedText = '';
    let updatedText = '';
    let plainText = this.text;
    const hilites = [];
    /* eslint-disable @typescript-eslint/quotes */
    const _HilightStart = "<span class='highlight'>";
    const _HilightEnd = '</span>';
    let start = plainText.indexOf(_HilightStart);
    while (start > -1) {
      const noHi = this.text.replace(_HilightStart, '');
      const ln = noHi.indexOf(_HilightEnd) - start;
      hilites.push({start, ln});
      plainText = noHi.replace(_HilightEnd, '');
      start = plainText.indexOf(_HilightStart);
    }
    if (plainText.length > this.limit) {
      this.isMore = true;
      shortenedText =  `${plainText.substring(0, this.limit)}`;
    } else {
      this.isMore = false;
      shortenedText =  plainText;
    }
    this.cleanText = plainText;
    // Now reinsert removed highlighting
    let i;
    updatedText = shortenedText;
    for (i = 0; i < hilites.length; i++) {
      if (hilites[i].start === 0) {
          if (hilites[i].ln >= shortenedText.length) {
            updatedText = _HilightStart + shortenedText + _HilightEnd;
          } else {
            updatedText = _HilightStart + shortenedText.substring(0, hilites[i].ln) + _HilightEnd + shortenedText.substring(hilites[i].ln);
          }
          updatedText = _HilightStart + shortenedText.substring(0, hilites[i].ln) + _HilightEnd + shortenedText.substring(hilites[i].ln);
      } else if (hilites[i].start > shortenedText.length) {
        updatedText = shortenedText;
      } else if (hilites[i].start < shortenedText.length - hilites[i].ln) {
        updatedText = shortenedText.substring(0, hilites[i].start) + _HilightStart +
          shortenedText.substring(hilites[i].start, hilites[i].start + hilites[i].ln) +
          _HilightEnd + shortenedText.substring(hilites[i].start + hilites[i].ln);
      } else if (hilites[i].start === shortenedText.length - hilites[i].ln) {
        updatedText = shortenedText.substring(0, hilites[i].start) + _HilightStart +
          shortenedText.substring(hilites[i].start) + _HilightEnd;
      } else {
        updatedText = shortenedText.substring(0, hilites[i].start) + _HilightStart +
          shortenedText.substring(hilites[i].start) + _HilightEnd;
      }
    }
    this.newText = updatedText;
    if (this.isMore === true) {
      this.newText += '<strong>...</strong>';
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['text'] || changes['limit']) {
      this.setText();
    }
  }
}

