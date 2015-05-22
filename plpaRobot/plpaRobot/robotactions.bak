#lang scheme


(define floorplan '(
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 3 3 3 0 )
(0 0 0 0 1 1 1 1 1 1 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 3 9 3 0 )
(0 0 0 0 0 4 10 4 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 4 4 4 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 4 4 11 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(2 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 0 0 )
(2 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 1 0 0 1 0 0 )
(2 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 1 0 0 1 0 0 )
(2 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 12 5 5 5 13 1 0 0 1 0 0 )
(2 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 1 0 0 1 0 0 )
(2 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 0 0 )
(2 1 1 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 6 6 14 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 6 6 6 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 6 6 6 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 6 6 6 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 6 6 15 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 0 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 7 16 7 0 )
(0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 7 7 7 0 )
))





(define tilestatus '((empty . 0 ) (path . 1) (park . 2) (ws0 . 3) (ws1 . 4) (ws2 . 5) (ws3 . 6) (ws4 . 7)
                     (ws0drop . 8) (ws0pick . 9)(ws1drop . 10) (ws1pick . 11) (ws2drop . 12) (ws2pick . 13)
                     (ws3drop . 14) (ws3pick . 15) (ws4drop . 16) (ws4pick . 17)))

(define robotstatus '((x . 0) (y . 0) (dir . 0) (carries . 0) (carriesTo . 0)))

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
          (setVariable 'y y 0 '()))
        (begin
          (if (= x (- (length (car floorplan)) 1))
              (begin
                (if (= y (- (length floorplan) 1))
                    "No parking found for initializing robot"
                    (initRobot 0 (+ y 1))))
              (initRobot (+ x 1) y))))))
  

(define (moveUp)
  (if
   (or (= (getY) 0)                                                                   ;check to see if at edge => fail = errormessage
        (and (not (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName 'path)))  ;check if tile y-1 is movable => fail = errormessage
             (not (= (getTileValueXY (getX) (- (getY) 1)) (getTileValueName 'park)))
             ))
      "Some error"
      (begin
        (setVariable 'y (- (getY) 1) 0 (list))
        (list (getX) (getY)))))

(define (moveDown)
  (if
    (or (= (getY) (length floorplan))                                       ;check to see if at edge => fail = errormessage
        (and (not (= (getTileValueXY (getX) (+ (getY) 1)) (getTileValueName 'path)))  ;check if tile y+1 is movable => fail = errormessage
             (not (= (getTileValueXY (getX) (+ (getY) 1)) (getTileValueName 'park)))
             ))
       "Some error"
       (begin
        (setVariable 'y (+ (getY) 1) 0 (list))
        (list (getX) (getY)))))


(define (moveRight)
    (if
     (or (= (getX) (length (car floorplan)))                                           ;check to see if at edge => fail = errormessage
        (and (not (= (getTileValueXY (+ (getX) 1) (getY) ) (getTileValueName 'path)))  ;check if tile x+1 is movable => fail = errormessage
             (not (= (getTileValueXY (+ (getX) 1) (getY) ) (getTileValueName 'park)))
             ))
       "Some error"
       (begin
        (setVariable 'x (+ (getX) 1) 0 (list))
        (list (getX) (getY)))))

(define (moveLeft)
    (if
      (or (= (getX) 0)                                                                 ;check to see if at edge => fail = errormessage
        (and (not (= (getTileValueXY (- (getX) 1) (getY) ) (getTileValueName 'path)))  ;check if tile x+1 is movable => fail = errormessage
             (not (= (getTileValueXY (- (getX) 1) (getY) ) (getTileValueName 'park)))
             ))
       "Some error"
       (begin
        (setVariable 'x (+ (getX) 1) 0 (list))
        (list (getX) (getY)))))

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
        
 