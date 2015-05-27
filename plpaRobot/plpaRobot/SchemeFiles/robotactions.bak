(define dirvalues '((up . 0) (right . 1) (down . 2) (left . 3) ))

(define tilestatus '((empty . 0 ) (path . 1) (park . 2) (ws0 . 3) (ws1 . 4) (ws2 . 5) (ws3 . 6) (ws4 . 7)
                     (ws0drop . 8) (ws0pick . 9)(ws1drop . 10) (ws1pick . 11) (ws2drop . 12) (ws2pick . 13)
                     (ws3drop . 14) (ws3pick . 15) (ws4drop . 16) (ws4pick . 17)))

(define robotstatus '((x . 0) (y . 8) (dir . 0) (carries . 0) (carriesTo . 0)))

(define (getX)
  (cdr(assoc 'x robotstatus)))

(define (getY)
  (cdr(assoc 'y robotstatus)))

(define (getDir)
  (cdr(assoc 'dir robotstatus)))

(define (getCarries)
  (cdr(assoc 'carries robotstatus)))

(define (getCarriesTo)
  (cdr(assoc 'carriesTo robotstatus)))
  
(define getTileValueXY
  (lambda (x y)
  (list-ref (list-ref floorplan y) x)))

(define getTileValueName
  (lambda (name)
    (cdr(assoc name tilestatus))))
  

(define setVariable
  (lambda (var val index newlist)
    (if (=  index (length robotstatus))
        (set! robotstatus newlist)
        (begin
        (if (eq? var (car(list-ref robotstatus index)))
            (begin
             (set! newlist (append  newlist (list (cons var val))))
             (setVariable var val (+ index 1) newlist))
            (begin
             (set! newlist (append newlist (list (list-ref robotstatus index))))
             (setVariable var val (+ index 1) newlist)))
        ))))


(define initRobot
  (lambda (x y)
    (if (= (getTileValueXY x y) (getTileValueName 'park))
        (begin
          (setVariable 'x x 0 '())
          (setVariable 'y y 0 '())
          (list (getX) (getY)))
        (begin
          (if (= x (- (length (car floorplan)) 1))
              (begin
                (if (= y (- (length floorplan) 1))
                    "No parking found for initializing robot"
                    (initRobot 0 (+ y 1))))
              (initRobot (+ x 1) y))))))
 

  



(define moveRight
  (lambda (times)
    (moveRightHelper times (list))))

(define moveRightHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin
          (if
           (or (= (getX) (length (car floorplan)))                                           ;check to see if at edge => fail = errormessage
               (and (not (= (getTileValueXY (+ (getX) 1) (getY) ) (getTileValueName 'path)))  ;check if tile x+1 is movable => fail = errormessage
                    (not (= (getTileValueXY (+ (getX) 1) (getY) ) (getTileValueName 'park)))
                    ))
           (moveRightHelper (- times 1) (append resultlist (list (list "Error: left is not a valid move direction"))))
           (begin
             (setVariable 'x (+ (getX) 1) 0 (list))
            (moveRightHelper (- times 1) (append resultlist (list (list "pos" (getX) (getY))))))))
        resultlist)))


	  
(define moveDown
  (lambda (times)
    (moveDownHelper times (list))))

(define moveDownHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin
          (if
           (or (= (getY) (length floorplan))                                       ;check to see if at edge => fail = errormessage
               (and (not (= (getTileValueXY (getX) (+ (getY) 1)) (getTileValueName 'path)))  ;check if tile y+1 is movable => fail = errormessage
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
           (or (= (getY) 0)                                                                   ;check to see if at edge => fail = errormessage
               (and (not (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName 'path)))  ;check if tile y-1 is movable => fail = errormessage
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
           (or (= (getX) 0)                                                                 ;check to see if at edge => fail = errormessage
               (and (not (= (getTileValueXY (- (getX) 1) (getY) ) (getTileValueName 'path)))  ;check if tile x+1 is movable => fail = errormessage
                    (not (= (getTileValueXY (- (getX) 1) (getY) ) (getTileValueName 'park)))
                    ))
           (moveLeftHelper (- times 1) (append resultlist (list (list "Error: left is not a valid move direction"))))
           (begin
            (setVariable 'x (- (getX) 1) 0 (list))
            (moveLeftHelper (- times 1) (append resultlist (list (list "pos" (getX) (getY))))))))
        resultlist)))

(define turnLeft
  (lambda (times)
    (turnLeftHelper times (list))))

(define turnLeftHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin 
          (if (= (getDir) 0)
              (setVariable 'dir 3 0 '())
              (setVariable 'dir (- (getDir) 1) 0 '()))        
          (turnLeftHelper (- times 1) (append resultlist (list (list "dir" (getDir))))))
        resultlist)))
  
(define turnRight
  (lambda (times)
    (turnRightHelper times (list))))

(define turnRightHelper
  (lambda (times resultlist)
    (if (> times 0)
        (begin 
          (if (= (getDir) 3)
              (setVariable 'dir 0 0 '())
              (setVariable 'dir (+ (getDir) 1) 0 '()))          
          (turnRightHelper (- times 1) (append resultlist (list (list "dir" (getDir))))))
        resultlist)))

        

(define adjacentspots
  (lambda (lookfor)
    (cond
      ((if (<= 0 (- (getX) 1)) ;check if at edge (don't look further)
           (begin
             (if (= (getTileValueXY (- (getX) 1) (getY)) (getTileValueName lookfor)) ;if not at edge check if tile to the left is looked for tile
                 #t ; true for the condition
                 #f)) ; fallse for the condition
           #f) #t) ; at edge, false for the condition -> not at edge and tile at left is correct tile, return true
      ((if (>= (length (car floorplan)) (+ (getX) 1))
           (begin
             (if (= (getTileValueXY (+ (getX) 1) (getY)) (getTileValueName lookfor))
                 #t
                 #f))
           #f) #t)      
      ((if (<= 0 (- (getY) 1))
           (begin
             (if (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName lookfor))
                 #t
                 #f))
           #f) #t)
      ((if (>= (length floorplan) (+ (getY) 1))
           (begin
             (if (= (getTileValueXY (getX) (+ (getY) 1)) (getTileValueName lookfor))
                 #t
                 #f))
           #f) #t) 
      (else #f))))       

(define lookInDirection
  (lambda (lookfor)
    (cond ((= (getDir) (cdr (assoc 'left dirvalues)))
           (begin
             (if (= (getX) 0)
                 #f
                 (begin
                   (if (= (getTileValueXY (- (getX) 1) (getY)) (getTileValueName lookfor))
                       #t
                       #f)))))
          ((= (getDir) (cdr (assoc 'up dirvalues)) )
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
  
(define (pickup)
    (if (= (getCarries) 0)
        (begin
          (let ((ws 'ws0PickUp))
            (cond
              ((= (getCarriesTo) 0) (set! ws 'ws0pick)) 
              ((= (getCarriesTo) 1) (set! ws 'ws1pick))
              ((= (getCarriesTo) 2) (set! ws 'ws2pick))
              ((= (getCarriesTo) 3) (set! ws 'ws3pick))
              ((= (getCarriesTo) 4) (set! ws 'ws4pick)))
            (if (adjacentspots ws)
                (begin
                  (setVariable 'carriesTo (+ (getCarriesTo) 1) 0 '())
                  (setVariable 'carries 1 0 '())
                  (getCarriesTo))
                "Not correct pickup point")))
        "Allready carries package"))
        
    ;look in adjacent spots for pickup
    ;check if pickup matches carriesto and carries is 0
      ;if matches pick up, set carries to 1
      ;else error message
    ;Increment carriesto to
      ;if 4 go to 0
      ;else ++
    ;
;    )
(define (dropoff)
  (if (= (getCarries) 1)
        (begin
          (let ((ws 'ws0drop))
            (cond
              ((= (getCarriesTo) 0) (set! ws 'ws0drop)) 
              ((= (getCarriesTo) 1) (set! ws 'ws1drop))
              ((= (getCarriesTo) 2) (set! ws 'ws2drop))
              ((= (getCarriesTo) 3) (set! ws 'ws3drop))
              ((= (getCarriesTo) 4) (set! ws 'ws4drop)))
            (if (adjacentspots ws)
                (begin
                  (if (= (getCarriesTo) 4)
                     (setVariable 'carriesTo 0 0 '())
                     (void))
                  (setVariable 'carries 0 0 '())
                  (getCarriesTo))
                "Not correct dropoff point")))
        "Does not carry anything"))
   ;look in adjacent spots for dropoff
   ;check if dropoff matches carriesto and carries is 1
      ;if matches drop of, set carries to 0
      ;else error message
;)

(define runProgram
  (lambda (programString)
     (let ((functionList (str-split programString #\newline)))
       (if (pair? functionList)
           (map evalFunctionInString functionList)
           (void))
       )))
    
    

(define evalFunctionInString
  (lambda expression
     ;(read (open-input-string (string-trim (car expression))))))
     (eval (read (open-input-string (string-trim (car expression)))) (interaction-environment))))


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
    
     

	 (define moveForward
  (lambda (times)
    (cond
      ((= (getDir) (cdr (assoc 'left dirvalues))) (moveLeft times))
      ((= (getDir) (cdr (assoc 'up dirvalues))) (moveUp times))
      ((= (getDir) (cdr (assoc 'right dirvalues))) (moveRight times))
      ((= (getDir) (cdr (assoc 'down dirvalues))) (moveDown times))
)))

(define moveBackward
  (lambda (times)
    (cond
      ((= (getDir) (cdr (assoc 'right dirvalues))) (+ 1 times))
      ((= (getDir) (cdr (assoc 'down dirvalues))) (moveUp times))
      ((= (getDir) (cdr (assoc 'left dirvalues))) (moveRight times))
      ((= (getDir) (cdr (assoc 'up dirvalues))) (moveDown times))
)))
        
 