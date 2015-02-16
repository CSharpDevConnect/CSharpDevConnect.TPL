The JSON contained in enrollments.json in this folder was generated using the following 
template on [JSON-Generator](http://www.json-generator.com/).


    [
      '{{repeat(9000, 10000)}}',
      {
        user: {
            userId: '{{guid()}}',
            firstName: '{{firstName()}}',
            lastName: '{{surname()}}',
            userName: function (tags) {
                 return (this.firstName + '.' + this.lastName + '@cSharpDevConnect.net');
            }
        },
        course: {
          courseId : '{{random("F55938A1-257E-45A1-B433-AB19D234EE2F", "6088136C-091C-43CE-A8AA-58CA88A63CC0", "7CA0D057-79C1-40C4-9624-0DEFD6AE077C")}}',
          courseName : function (tags) {
                          var names = ['MATH-101', 'ENG-245', 'CS-200'];
                          var nameIndex =0;
                          if (this.courseId == "6088136C-091C-43CE-A8AA-58CA88A63CC0"){
                              nameIndex = 1;
                          }
        
                          if (this.courseId == "7CA0D057-79C1-40C4-9624-0DEFD6AE077C"){
                             nameIndex = 2;
                          }
                          return names[nameIndex];
                     }
        }
      }
    ]
