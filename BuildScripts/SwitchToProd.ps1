(Get-Content ..\ImprovedSchedulingSystemApi\ImprovedSchedulingSystemApi\wwwroot\index.html).replace('<!-- In production use:
  <script src=\"//ajax.googleapis.com/ajax/libs/angularjs/x.x.x/angular.min.js\"></script>
  -->', '<script src=\"//ajax.googleapis.com/ajax/libs/angularjs/x.x.x/angular.min.js\"></script>') | Set-Content ..\ImprovedSchedulingSystemClient\app\index.html