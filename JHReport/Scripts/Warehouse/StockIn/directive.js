app.directive('onRepeatFinishedRender', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    //这里element, 就是ng-repeat渲染的最后一个元素
                    scope.$emit('ngRepeatFinished', element);
                });
            }
        }
    };
});