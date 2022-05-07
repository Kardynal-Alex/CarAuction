import { ComponentRef, Directive, ElementRef, HostListener, Input, OnInit, TemplateRef } from '@angular/core';
import { Overlay, OverlayPositionBuilder, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';

import { AwesomeTooltipComponent } from './tooltip.component';

@Directive({ selector: '[awesomeTooltip]' })
export class AwesomeTooltipDirective implements OnInit {
    @Input('awesomeTooltip') text = '';
    private overlayRef: OverlayRef;
    @Input() showToolTip: boolean = true;
    @Input() contentTemplate: TemplateRef<any>;

    constructor(
        private overlay: Overlay,
        private overlayPositionBuilder: OverlayPositionBuilder,
        private elementRef: ElementRef
    ) { }

    ngOnInit(): void {
        if (!this.showToolTip) return;

        const positionStrategy = this.overlayPositionBuilder
            .flexibleConnectedTo(this.elementRef)
            .withPositions([{
                originX: 'center',
                originY: 'top',
                overlayX: 'center',
                overlayY: 'bottom',
                offsetY: -8,
            }]);

        this.overlayRef = this.overlay.create({ positionStrategy });
    }

    @HostListener('mouseenter')
    show() {
        if (this.overlayRef && !this.overlayRef.hasAttached()) {
            const tooltipRef: ComponentRef<AwesomeTooltipComponent>
                = this.overlayRef.attach(new ComponentPortal(AwesomeTooltipComponent));
            tooltipRef.instance.text = this.text;
            tooltipRef.instance.contentTemplate = this.contentTemplate;
        }
    }

    @HostListener('mouseout')
    hide() {
        this.closeToolTip();
    }

    ngOnDestroy() {
        this.closeToolTip();
    }

    private closeToolTip() {
        if (this.overlayRef) {
            this.overlayRef.detach();
        }
    }
}

/*
TODO: How to use in simple way
<i class="fa fa-edit" awesomeTooltip="Edit lot" [showToolTip]="false"></i>
///////////////////////////////////////////////////////////////////////////////
TODO use tooltip directive with ng-template
<button><i class="fa fa-close" awesomeTooltip [contentTemplate]="tooltipTemplate"></i></button>
<ng-template #tooltipTemplate>
    <div>
        <p>Close Bid</p>
        <p>Start price</p>
    </div>
</ng-template>
*/