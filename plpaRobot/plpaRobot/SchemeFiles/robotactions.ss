;The look-up list for the directional values, by name indentifier
(define dirvalues '((up . 0) (right . 1) (down . 2) (left . 3) )) 

;the look-up list for the tile values, by name;
(define tilestatus '((empty . 0 ) (path . 1) (park . 2) (ws0 . 3) (ws1 . 4) (ws2 . 5) (ws3 . 6) (ws4 . 7)
                     (ws0drop . 8) (ws0pick . 9)(ws1drop . 10) (ws1pick . 11) (ws2drop . 12) (ws2pick . 13)
                     (ws3drop . 14) (ws3pick . 15) (ws4drop . 16) (ws4pick . 17))) 


;the current state of the robot, values by name identifier
(define robotstatus '((x . 0) (y . 8) (dir . 0) (carries . 0) (carriesTo . 0)))

;-------- ROBOT STATE AND FLOORPLAN INTERACTION VARIABLES --------------
;returns the x value of the position of the robot
(define (getX)
  (cdr(assoc 'x robotstatus)))

;returns the y value of the position of the robot
(define (getY)
  (cdr(assoc 'y robotstatus)))

;returns the direciton of the robot
(define (getDir)
  (cdr(assoc 'dir robotstatus)))

;returns the value deciding wheter or not the robot is carrying an item
(define (getCarries)
  (cdr(assoc 'carries robotstatus)))

;returns the value defining the workstation the robot should interact with next
(define (getCarriesTo)
  (cdr(assoc 'carriesTo robotstatus)))
  
;returns the value of the tile at position x y
(define getTileValueXY
  (lambda (x y)
  (list-ref (list-ref floorplan y) x)))

;returns the value of the tile by name.
(define getTileValueName
  (lambda (name)
    (cdr(assoc name tilestatus))))

  
;set a given variable of the robot state
;var = variable to set, val = value to set, index = index of next item in robot state, recursion use; newlist = list to return with updated values, recursion use.
(define setVariable
  (lambda (var val index newlist)
    (if (=  index (length robotstatus)) ;stop if final variable has been processed
        (set! robotstatus newlist); if final variable has been procesed update list
        (begin 
        (if (eq? var (car(list-ref robotstatus index))) ;if item is the one to be updated
            (begin ;if variiable
             (set! newlist (append  newlist (list (cons var val)))) ;  append newlist with variable and value
             (setVariable var val (+ index 1) newlist)) ;next index recursion
            (begin ;if not
             (set! newlist (append newlist (list (list-ref robotstatus index))));set new list with data from old list
             (setVariable var val (+ index 1) newlist))) ;next index reursion
        ))))

;Initializes robot. Finds a parking space and resets the values of the robot
;x = initial search x-coordinate for floorplan, y = intial search y-coordinate for floorplan
(define initRobot 
  (lambda (x y)
    (if (= (getTileValueXY x y) (getTileValueName 'park)) ;If tile is a park til
        (begin ;set all variables
          (setVariable 'x x 0 '()) 
          (setVariable 'y y 0 '()) 
          (setVariable 'dir  0 0 '())
          (setVariable 'carriesTo 0 0 '())
          (setVariable 'carries 0 0 '())
          (list (list "pos" (getX) (getY)))) ;return values list with position, x and y.
        (begin;check next spot
          (if (= x (- (length (car floorplan)) 1)) ;if at end of current row
              (begin
                (if (= y (- (length floorplan) 1)) ;if at end of current column
                    "Error: No parking found for initializing robot" ;no parking space found, returns error
                    (initRobot 0 (+ y 1)))) ;recursion with new coordinate
              (initRobot (+ x 1) y)))))) 

;---------------------------------------------;
;------------ ROBOT MOVEMENT -----------------;  

;ALL ABSOLUTE MOVEMENTS ARE CREATED THE SAME WAY. SEE MOVERIGHT FOR COMMENTS!

;move the robot in an absolute way to the right
;times = number of times to move
(define moveRight
  (lambda (times)
    (moveRightHelper times (list)))) ;call helper with empty number of times and an empty result list

;helper function for recursion calls of moveright
;times = number of times to move, resultlist = the list that contains results of movements.
(define moveRightHelper
  (lambda (times resultlist)
    (if (> times 0) ;Move by recursion multiple times, based on times parameter. 
        (begin ;
          (if ;check if tile is movable and not at an edge
           (or (= (getX) (length (car floorplan)))                                           
               (and (not (= (getTileValueXY (+ (getX) 1) (getY) ) (getTileValueName 'path)))  
                    (not (= (getTileValueXY (+ (getX) 1) (getY) ) (getTileValueName 'park)))
                    ))
           (moveRightHelper (- times 1) (append resultlist (list (list "Error: left is not a valid move direction")))) ;append error messaage to result list
           (begin
             (setVariable 'x (+ (getX) 1) 0 (list)) ;if valid direction, increase x by one and do recursion
            (moveRightHelper (- times 1) (append resultlist (list (list "pos" (getX) (getY)))))))) ;append result with an index of "pos"
        resultlist))); when moved the right number of times, return result list


	  
(define moveDown
  (lambda (times)
    (moveDownHelper times (list))))

(define moveDownHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin
          (if
           (or (= (getY) (length floorplan))                                     
               (and (not (= (getTileValueXY (getX) (+ (getY) 1)) (getTileValueName 'path)))  
                    (not (= (getTileValueXY (getX) (+ (getY) 1)) (getTileValueName 'park)))))
           (moveDownHelper (- times 1) (append resultlist (list (list "Error: down is not a valid move direction"))))
           (begin
             (setVariable 'y (+ (getY) 1) 0 (list))
             (moveDownHelper (- times 1) (append resultlist (list (list "pos" (getX) (getY))))))))
        resultlist)))
		
(define moveUp
  (lambda (times)
    (moveUpHelper times (list))))

(define moveUpHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin
          (if
           (or (= (getY) 0)                                                                  
               (and (not (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName 'path)))  
                    (not (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName 'park)))))
           (moveUpHelper (- times 1) (append resultlist (list (list "Error: up is not a valid move direction"))))
           (begin
             (setVariable 'y (- (getY) 1) 0 (list))
             (moveUpHelper (- times 1) (append resultlist (list (list "pos" (getX) (getY))))))))
        resultlist)))
		
(define moveLeft
  (lambda (times)
    (moveLeftHelper times (list))))

(define moveLeftHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin
          (if
           (or (= (getX) 0)                                                                 
               (and (not (= (getTileValueXY (- (getX) 1) (getY) ) (getTileValueName 'path)))  
                    (not (= (getTileValueXY (- (getX) 1) (getY) ) (getTileValueName 'park)))
                    ))
           (moveLeftHelper (- times 1) (append resultlist (list (list "Error: left is not a valid move direction"))))
           (begin
            (setVariable 'x (- (getX) 1) 0 (list))
            (moveLeftHelper (- times 1) (append resultlist (list (list "pos" (getX) (getY))))))))
        resultlist)))

;Used for relative movement based on the direction the robot is facing. Moves the robot foward one step.
;times = number of times to move.
(define moveForward
  (lambda (times)
    (cond 
      ((= (getDir) (cdr (assoc 'left dirvalues))) (moveLeft times))
      ((= (getDir) (cdr (assoc 'up dirvalues))) (moveUp times))
      ((= (getDir) (cdr (assoc 'right dirvalues))) (moveRight times))
      ((= (getDir) (cdr (assoc 'down dirvalues))) (moveDown times))
)))

;Used for relative movement baesd on the direction robot is facing. Moves the robot backward a number of steps.
;times = number of times to move
(define moveBackward
  (lambda (times)
    (cond ;Same procedure as moveForward, however here the procedure opposite of the current direction is chosen.
      ((= (getDir) (cdr (assoc 'right dirvalues))) (moveLeft times))
      ((= (getDir) (cdr (assoc 'down dirvalues))) (moveUp times))
      ((= (getDir) (cdr (assoc 'left dirvalues))) (moveRight times))
      ((= (getDir) (cdr (assoc 'up dirvalues))) (moveDown times))
)))

;-----------------------------------------------------------;


;-------------------- TURNING THE ROBOT --------------------;

;turns the robot in a counterclockwise maner a number of times
;times = the number of times to turn.
(define turnLeft
  (lambda (times)
    (turnLeftHelper times (list)))) ;call the helper function with an empty result list

;helper functions for turnLeft. Handles the turning itself. 
;times = the number of times to turn, resultlist = the list that contains all the results of the operations.
(define turnLeftHelper
  (lambda (times resultlist)
    (if (> times 0) ; continue until while number of times is not met.
        (begin 
          (if (= (getDir) 0)
              (setVariable 'dir 3 0 '()) ;if direction is 0 (up) set to 3 (left)
              (setVariable 'dir (- (getDir) 1) 0 '())) ;else decrement direction       
          (turnLeftHelper (- times 1) (append resultlist (list (list "dir" (getDir)))))) ;append result to resultlist with and index of "dir"
        resultlist))) ;return resutlist after concluded recursion
  
;See turnLeft.
(define turnRight
  (lambda (times)
    (turnRightHelper times (list))))

;See turnLeftHelper.
(define turnRightHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin 
          (if (= (getDir) 3) ;only difference. Set to 0 (up) if direction is 3 (left). Else increment
              (setVariable 'dir 0 0 '())
              (setVariable 'dir (+ (getDir) 1) 0 '()))          
          (turnRightHelper (- times 1) (append resultlist (list (list "dir" (getDir))))))
        resultlist)))

;-----------------------------------------------;
;--------- PICKING UP AND DROPPING OFF----------;
;Checks to see if the tile type in the direction the robot is facing, is the same as the one in the argument
(define lookInDirection
  (lambda (lookfor)
    (cond ((= (getDir) (cdr (assoc 'left dirvalues))) ;If looking left
           (begin
             (if (= (getX) 0) ;if at edge the tile is not the same
                 #f 
                 (begin
                   (if (= (getTileValueXY (- (getX) 1) (getY)) (getTileValueName lookfor))
                       #t ;if value to the left is the same value as the one supplied.                                                                       
                       #f)))))
          ((= (getDir) (cdr (assoc 'up dirvalues)) ) ;same thing happens for the last three conditional statements. They simply look in other directions
           (begin
             (if (= (getY) 0)
                 #f
                 (begin
                   (if (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName lookfor))
                       #t
                       #f)))))
          ((= (getDir) (cdr (assoc 'right dirvalues)) )
           (begin
             (if (= (getX) (- (length (car floorplan)) 1))
                 #f
                 (begin
                   (if (= (getTileValueXY (+ (getX) 1) (getY)) (getTileValueName lookfor))
                       #t
                       #f)))))
          ((= (getDir) (cdr (assoc 'down dirvalues)) )
           (begin
             (if (= (getY) (- (length floorplan)) 1)
                 #f
                 (begin
                   (if (= (getTileValueXY (getX) (+ 1 (getY))) (getTileValueName lookfor))
                       #t
                       #f))))))))

;Pick up a package from a workstation.
(define (pickup)
    (if (= (getCarries) 0) ;cannot carry multiple pacakage. Check if already carrying
        (begin
          (let ((ws 'ws0PickUp))
            (cond  ;Set the workstation pickup point to search for, based on the carriesTo variable.
              ((= (getCarriesTo) 0) (set! ws 'ws0pick))
              ((= (getCarriesTo) 1) (set! ws 'ws1pick))
              ((= (getCarriesTo) 2) (set! ws 'ws2pick))
              ((= (getCarriesTo) 3) (set! ws 'ws3pick))
              ((= (getCarriesTo) 4) (set! ws 'ws4pick)))
            (if (lookInDirection ws) ;if the next tile in the direction the robot is facing, is the the correct workstation pickup point
                (begin
                  (setVariable 'carriesTo (+ (getCarriesTo) 1) 0 '()) ;dropoff at next workstation
                  (setVariable 'carries 1 0 '()) ;The robot is no longer carrying
                  (list (list "pickup" (getCarriesTo)))) ;return the result with the workstation as a value, and "pickup" as index
                "Error: Not correct pickup point"))) ;if not pickup point, set error message
        "Error: Allready carries package")) ;Return allready carrying message.

;Drop of a package at a workstation
(define (dropoff)
  (if (= (getCarries) 1) ;Cannot not dropoff package if not carrying.
        (begin
          (let ((ws 'ws0drop))
            (cond
              ((= (getCarriesTo) 0) (set! ws 'ws0drop)) 
              ((= (getCarriesTo) 1) (set! ws 'ws1drop))
              ((= (getCarriesTo) 2) (set! ws 'ws2drop))
              ((= (getCarriesTo) 3) (set! ws 'ws3drop))
              ((= (getCarriesTo) 4) (set! ws 'ws4drop)))
            (if (lookInDirection ws)
                (begin
                  (if (= (getCarriesTo) 4) ;if at final workstation. Next package to pick up is at the first one.
                     (setVariable 'carriesTo 0 0 '())
                     (void)) ;do nothing if not at final workstation
                  (setVariable 'carries 0 0 '())
                  (list (list "dropoff" (getCarriesTo)))); Return the result with the workstation as a value, and "dropoff" as index
                "Error: Not correct dropoff point"))) ;return not correct dropoff point error message
        "Error: Does not carry anything")) ;return not carrying anything error message

;-----------------------------------------------------------------------------
;-------------- INTERFACE PROCEDURES FOR EXTERNAL APLLICATION ----------------

;Runs a robot program, with commands delimited by newline
;programstring = The commands to run, delimted by newline
(define runProgram
  (lambda (programString)
           (map evalFunctionInString (str-split programString #\newline)))) ;call the evalFunctionInString, for all items in the list created by the string split.

;Evaluates the string argument.
;expression = the argument to evaluate
(define evalFunctionInString
  (lambda expression
    ;trim white-spaces from the expression, create an open-input-string from this, and read that. Use the input in an eval call.
     (eval (read (open-input-string (string-trim (car expression)))) (interaction-environment))))

;External string split procedure. Procured from Matthew Phillips (https://gist.github.com/matthewp/2324447)
;str = string to split, ch = delimiter.
(define (str-split str ch)
  (let ((len (string-length str)))
    (letrec
      ((split
        (lambda (a b)
          (cond
            ((>= b len) (if (= a b) '() (cons (substring str a b) '())))
              ((char=? ch (string-ref str b)) (if (= a b)
                (split (+ 1 a) (+ 1 b))
                  (cons (substring str a b) (split b b))))
                (else (split a (+ 1 b)))))))
                  (split 0 0))))
    
     


        
 