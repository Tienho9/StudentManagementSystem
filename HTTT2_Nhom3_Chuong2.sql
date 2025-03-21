----------------VIEW------------------
-- Nguyễn Thị Thu Hằng
/* hiển thị danh sách sinh viên đạt học bổng và số tiền nhận được trong kỳ 1 
(điều kiện đạt học bổnglà điểm trung bình các môn phải >= 8.5 
và số tín chỉ học tối thiểu là 6 tín)*/
CREATE VIEW Scholarship
AS
SELECT 
    S.StudentID, 
    S.StudentName, 
	S.Gender,
    AVG(SC.TotalScore) AS DiemTrungBinh,
    SUM(SB.Credits) AS TongTinChi,
    TF.TuitionAmount, 
    (TF.TuitionAmount + (TF.TuitionAmount * 0.2)) AS TienHocBong
FROM 
    Student S
JOIN 
    Score SC ON S.StudentID = SC.StudentID
JOIN 
    [Subject] SB ON SC.SubjectID = SB.SubjectID
JOIN 
    TuitionFee TF ON S.StudentID = TF.StudentID
WHERE 
    SB.Semester = 1 -- Lọc các môn học thuộc học kỳ 1
    AND TF.Semester = 1 -- Lọc học phí của học kỳ 1
GROUP BY 
    S.StudentID, S.StudentName, S.Gender, TF.TuitionAmount
HAVING 
    AVG(SC.TotalScore) >= 8.5    
    AND SUM(SB.Credits) >= 6;    

SELECT * FROM Scholarship

--View đưa ra thông tin sinh viên và lớp học

CREATE VIEW StudentClassInfo AS
SELECT 
    S.StudentID, 
    S.StudentName, 
    C.ClassName, 
    D.DepartmentName
FROM 
    Student S
JOIN 
    Class C ON S.ClassID = C.ClassID
JOIN 
    Department D ON S.DepartmentID = D.DepartmentID;


-- Hồ Thủy Tiên
-- Đưa ra mã sinh viên, tên sinh viên, lớp và tổng số tín chỉ đã học của sinh viên lớp 64CX1
CREATE VIEW StudentCredits AS
SELECT 
    s.StudentID, 
    s.StudentName, 
	s.ClassID,
    SUM(sub.Credits) AS TotalCredits
FROM 
    Student s
    INNER JOIN Score sc ON s.StudentID = sc.StudentID
    INNER JOIN [Subject] sub ON sc.SubjectID = sub.SubjectID
WHERE
	s.ClassID = '64CX1'
GROUP BY 
    s.StudentID, s.StudentName, s.ClassID

SELECT * FROM StudentCredits

--View đưa ra tổng số sinh viên của từng khoa
CREATE VIEW DepartmentStudentCount AS
SELECT 
    D.DepartmentID, 
    D.DepartmentName, 
    COUNT(S.StudentID) AS TotalStudents
FROM 
    Department D
LEFT JOIN 
    Student S ON D.DepartmentID = S.DepartmentID
GROUP BY 
    D.DepartmentID, D.DepartmentName;


--Khổng Thị Vân
-- Hiển thị danh sách các sinh viên có điểm tổng cao nhất trong từng môn học.
CREATE VIEW SinhVienDiemCaoNhat AS 
	SELECT 
		S.StudentID,
		S.StudentName,
		SB.SubjectName,
		SC.TotalScore
	FROM 
		Student S
	JOIN 
		Score SC ON S.StudentID = SC.StudentID
	JOIN 
		[Subject] SB ON SC.SubjectID = SB.SubjectID
	JOIN 		
		( SELECT 
				SubjectID,
				MAX(TotalScore) AS MaxTotalScore
		  FROM 
				Score
		  GROUP BY 
				SubjectID
		) MS ON SC.SubjectID = MS.SubjectID AND SC.TotalScore = MS.MaxTotalScore;

SELECT * FROM SinhVienDiemCaoNhat

--View đưa ra điểm chi tiết của sinh viên

CREATE VIEW StudentScoreInfo AS
SELECT 
    S.StudentID, 
    S.StudentName, 
    SB.SubjectName, 
    SC.TotalScore, 
    SC.LetterGrade
FROM 
    Student S
JOIN 
    Score SC ON S.StudentID = SC.StudentID
JOIN 
    [Subject] SB ON SC.SubjectID = SB.SubjectID;

-- Vũ Hà Lâm
-- Hiển thị thông tin danh sách sinh viên có điểm trung bình môn học của sinh viên lớp 64HTTT2 > 7
CREATE VIEW HighAverageScore AS
SELECT 
    st.StudentID,
    st.StudentName,
	st.Gender,
    st.ClassID,
    st.DepartmentID,
    AVG(sc.TotalScore) AS AvgTotalScore
FROM Student st JOIN Score sc
ON st.StudentID = sc.StudentID
WHERE st.ClassID = '64HTTT2'
GROUP BY st.StudentID,
    st.StudentName,
	st.Gender,
    st.ClassID,
    st.DepartmentID
HAVING AVG(sc.TotalScore) > 7

SELECT * FROM HighAverageScore

--View đưa ra học phí đã đóng của sinh viên

CREATE VIEW TuitionInfo AS
SELECT 
    S.StudentID, 
    S.StudentName, 
    T.Semester, 
    T.TuitionAmount
FROM 
    Student S
JOIN 
    TuitionFee T ON S.StudentID = T.StudentID;


-- Hoàng Khánh Hà
--Hiển thị thông tin sinh viên học môn học có mã CSE484
CREATE VIEW StudentAttemptSummaryView AS
SELECT 
	s.StudentID,
	st.StudentName,
	s.SubjectID,
	st.ClassID,
	st.DepartmentID,
	s.NumberOfAttempts
FROM Score s JOIN 
	Student st ON s.StudentID = st.StudentID
WHERE s.SubjectID = 'CSE484'

SELECT * FROM StudentAttemptSummaryView

-- View đưa ra những sinh viên nhận thưởng
sql
CREATE VIEW AwardedStudents AS
SELECT 
    S.StudentID, 
    S.StudentName, 
    A.Reason, 
    A.AwardAmount, 
    A.DecisionDate
FROM 
    Student S
JOIN 
    Award A ON S.StudentID = A.StudentID;



-----------------PROCEDURE---------------------
-- Nguyễn Thị Thu Hằng
--Thống kê danh sách và tổng số khen thưởng trong một khoảng thời gian nhất định

CREATE PROCEDURE GetAwardByDate
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        A.AwardID, 
        A.StudentID, 
        A.DecisionDate, 
        A.AwardAmount, 
        A.Reason,
        COUNT(A.AwardID) AS TotalAwards
    FROM Award A
    WHERE A.DecisionDate BETWEEN @StartDate AND @EndDate
    GROUP BY A.AwardID, A.StudentID, A.DecisionDate, A.AwardAmount, A.Reason
    UNION
    SELECT 
        NULL AS AwardID,
        NULL AS StudentID,
        NULL AS DecisionDate,
        NULL AS AwardAmount,
        N'Tổng số khen thưởng' AS Reason,
        COUNT(A.AwardID) AS TotalAwards
    FROM Award A
    WHERE A.DecisionDate BETWEEN @StartDate AND @EndDate;
END;

-- Thực thi procedure
EXEC GetAwardByDate
    @StartDate = '2023-08-01', 
    @EndDate = '2023-11-30';

-- Hồ Thủy Tiên
/*Thủ tục thêm môn học mới vào bảng môn học, kiểm tra môn học đã tồn tại chưa. 
Tín chỉ nằm trong khoảng 1-10, 
học kỳ nằm trong khoảng 1-8 và mô tả môn học không được để trống.*/

CREATE PROC AddSubject
    @SubjectID VARCHAR(8),
    @SubjectName NVARCHAR(100),
    @Credits INT,
    @Semester INT,
    @SubjectDescription NVARCHAR(255)
AS
BEGIN
    DECLARE @ExistingSubject INT;

    -- Kiểm tra xem môn học đã tồn tại hay chưa
    SELECT @ExistingSubject = COUNT(*)
    FROM [Subject] 
    WHERE SubjectID = @SubjectID;

    IF @ExistingSubject > 0
    BEGIN
        PRINT 'Course already exists.'
        RETURN
    END

    IF @Credits < 1 OR @Credits > 10
    BEGIN
        PRINT 'Credits must be between 1 and 10.'
        RETURN
    END
	
	IF @Semester < 1 OR @Semester > 8
    BEGIN
        PRINT 'Semester must be between 1 and 8.'
        RETURN
    END
   
    IF @SubjectDescription IS NULL OR LEN(@SubjectDescription) = 0
    BEGIN
        PRINT 'Subject description cannot be empty.'
        RETURN
    END

    INSERT INTO [Subject] (SubjectID, SubjectName, Credits, Semester, SubjectDescription)
    VALUES (@SubjectID, @SubjectName, @Credits, @Semester, @SubjectDescription)
    
    PRINT 'Add ' + @SubjectID + ' Successfully'
END
GO
-- Thực thi
Exec AddSubject 'CSE486', N'Hệ quản trị cơ sở dữ liệu',3,5,N'Tổng quan về hệ quản trị cơ sở dữ liệu Microsoft SQL Server.';

Exec AddSubject 'English1', N'Tiếng anh 1',3,3,N'Cung cấp kiến thức cơ bản về cấu trúc và các thì trong tiếng anh.';

Exec AddSubject 'CNXH', N'Chủ nghĩa xã hội',3,15,N'Kiến thức chuyên sâu về chủ nghĩa xã hội.';

-- Khổng Thị Vân
-- Procedure cập nhật điểm sinh viên.

CREATE PROCEDURE sp_CapNhatDiem
    @StudentID VARCHAR(10),
    @SubjectID VARCHAR(8),
    @ContinuousAssessmentScore FLOAT,
    @FinalExamScore FLOAT
AS
BEGIN
    DECLARE @TotalScore FLOAT;
    DECLARE @LetterGrade NVARCHAR(2);
	DECLARE @Status VARCHAR(20);
	DECLARE @StudentExists INT;

	SELECT @StudentExists = COUNT(*)
    FROM Student
    WHERE StudentID = @StudentID;

    IF @StudentExists = 0
    BEGIN
        PRINT N'StudentID không tồn tại';
        RETURN;
	end;

  
    SET @TotalScore = (@ContinuousAssessmentScore * 0.4) + (@FinalExamScore * 0.6);

    
    SET @LetterGrade = CASE 
        WHEN @TotalScore >= 8.5 AND @TotalScore <= 10 THEN 'A'
        WHEN @TotalScore >= 7 AND @TotalScore < 8.5 THEN 'B'
        WHEN @TotalScore >= 5.5 AND @TotalScore < 7 THEN 'C'
        WHEN @TotalScore >= 4 AND @TotalScore < 5.5 THEN 'D'
        ELSE 'F' 
    END;

	SET @Status = CASE 
        WHEN @TotalScore < 4 THEN 'Fail' 
        ELSE 'Pass' 
    END;

   
    UPDATE Score
    SET ContinuousAssessmentScore = @ContinuousAssessmentScore, 
        FinalExamScore = @FinalExamScore, 
        TotalScore = @TotalScore,
        LetterGrade = @LetterGrade,
		[Status] = @Status
    WHERE StudentID = @StudentID AND SubjectID = @SubjectID;

END;
GO

EXEC sp_CapNhatDiem 
    @StudentID = '22',               
    @SubjectID = 'CSE481',              
    @ContinuousAssessmentScore = 8.6,
    @FinalExamScore = 10.0;

-- Vũ Hà Lâm
-- Procudure cập nhật tên sinh viên và ngày sinh của sinh viên

CREATE PROC UpdateStudent
    @StudentID VARCHAR(10),
    @StudentName NVARCHAR(100), 
    @DateOfBirth DATE  
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Student WHERE StudentID = @StudentID)
    BEGIN
        UPDATE Student
        SET StudentName = @StudentName,
            DateOfBirth = @DateOfBirth
        WHERE StudentID = @StudentID;
        PRINT 'Student information updated successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Student not found with the provided StudentID.';
    END
END;

-- Thực thi
EXEC UpdateStudent
    @StudentID = '2251162900',        
    @StudentName = 'Vũ Chung Kiên', 
	@DateOfBirth = '2000-04-09'

-- Hoàng Khánh Hà
-- Nhập mã sinh viên hiển thị thông tin sinh viên gồm tên, lớp , khoa

CREATE PROC GetStudentInfo
   @StudentID VARCHAR(10) 
AS
BEGIN
  
   IF EXISTS (SELECT 1 FROM Student WHERE StudentID = @StudentID)
   BEGIN
       
       SELECT 
           S.StudentName AS [Student Name], 
           C.ClassName AS [Class], 
           D.DepartmentName AS [Department]
       FROM Student S
       JOIN Class C ON S.ClassID = C.ClassID
       JOIN Department D ON S.DepartmentID = D.DepartmentID
       WHERE S.StudentID = @StudentID;
   END
   ELSE
   BEGIN
       PRINT 'Student not found with the provided StudentID.';
   END
END;
GO
exec GetStudentInfo '2251162034'


-----------------FUNCTION---------------------
-- Nguyễn Thị Thu Hằng
-- tính đểm gpa của sinh viên theo mã sinh viên
DROP FUNCTION fnCalculateGPA
CREATE FUNCTION fnCalculateGPA (
    @StudentID VARCHAR(10)
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @TotalScore FLOAT = 0, @TotalCredits INT = 0, @GPA FLOAT;
    
    SELECT @TotalScore = SUM(s.TotalScore * sub.Credits),
           @TotalCredits = SUM(sub.Credits)
    FROM Score s
    JOIN [Subject] sub ON s.SubjectID = sub.SubjectID
    WHERE s.StudentID = @StudentID;

    IF @TotalCredits = 0
        SET @GPA = 0;
    ELSE
        SET @GPA = @TotalScore / @TotalCredits;

    RETURN @GPA;
END;

----------------------------------
-- Hồ Thủy Tiên
-- Hàm đưa ra chi tiết về môn học, tín chỉ và học phí từng môn của sinh viên theo kì
CREATE FUNCTION TTSV
    (@StudentID VARCHAR(10), @Semester INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        s.SubjectID,
        s.SubjectName,
        s.Credits,
        tf.StudentID,
        tf.Semester,
        (s.Credits*300000) AS Tuition
    FROM
        [Subject] s
    JOIN
        Score sc ON s.SubjectID = sc.SubjectID
    JOIN
        TuitionFee tf ON sc.StudentID = tf.StudentID AND s.Semester = tf.Semester
    WHERE
        tf.StudentID = @StudentID AND tf.Semester = @Semester
);
GO
Select * from TTSV('2251162015', 1)


-- Khổng Thị Vân
-- Function phân loại sinh viên theo điểm trung bình

CREATE FUNCTION ClassifyStudentByTotalScore(@StudentID VARCHAR(10))
RETURNS NVARCHAR(20)
AS
BEGIN
    DECLARE @TotalScore FLOAT;
    DECLARE @Classification NVARCHAR(20);

    SELECT @TotalScore = SUM(TotalScore * Credits) / SUM(Credits)
    FROM Score 
    JOIN [Subject] ON Score.SubjectID = [Subject].SubjectID
    WHERE StudentID = @StudentID;

    IF @TotalScore >= 8.5
        SET @Classification = N'Giỏi';
    ELSE IF @TotalScore >= 7.0
        SET @Classification = N'Khá';
    ELSE IF @TotalScore >= 5.5
        SET @Classification = N'Trung bình';
    ELSE 
        SET @Classification = N'Yếu';
    RETURN @Classification;
END;
SELECT dbo.ClassifyStudentByTotalScore('2251161987') AS [Classification];

-- Vũ Hà Lâm
-- trả về những sinh viên đã PASS môn CSE204 của lớp 64HTTT2

CREATE FUNCTION dbo.GetPassedStudents
(
    @SubjectID VARCHAR(8),
    @ClassID VARCHAR(8)
)
RETURNS TABLE
AS
RETURN
(
    SELECT st.StudentID, st.StudentName, st.ClassID, s.TotalScore, s.Status
    FROM Student st
    JOIN Score s ON st.StudentID = s.StudentID
    WHERE s.SubjectID = @SubjectID 
      AND st.ClassID = @ClassID 
      AND s.Status = 'Pass'
)

SELECT * 
FROM dbo.GetPassedStudents('CSE204', '64HTTT2');

-- Hoàng Khánh Hà
-- Trả về danh sách môn học của sinh viên lớp 64HTTT2
CREATE FUNCTION dbo.GetSubjectsByClass
(
    @ClassID VARCHAR(8)
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT s.SubjectID, sb.SubjectName, sb.Credits, sb.Semester, sb.SubjectDescription
    FROM Student st
    JOIN Score s ON st.StudentID = s.StudentID
    JOIN [Subject] sb ON s.SubjectID = sb.SubjectID
    WHERE st.ClassID = @ClassID
)
GO
SELECT * FROM dbo.GetSubjectsByClass('64HTTT2');



-----------------TRIGGER---------------------
-- Nguyễn Thị Thu Hằng
/* Cập nhật điểm thi hết môn của sinh viên 
và sẽ gửi thông báo cảnh báo khi sinh viên có điểm hết môn < 4.*/
DROP TRIGGER trgSendWarning
CREATE TRIGGER trgSendWarning
ON Score
AFTER UPDATE
AS
BEGIN
    DECLARE @StudentID VARCHAR(10);
    DECLARE @NewScore FLOAT;

    SELECT @StudentID = i.StudentID, @NewScore = i.TotalScore 
    FROM inserted i
    WHERE i.TotalScore < 4.0;

    IF @NewScore < 4.0
    BEGIN
        PRINT N'Cảnh báo: Sinh viên ' + @StudentID + N' có điểm số dưới mức yêu cầu!';
    END
END;

UPDATE Score
SET TotalScore = 3.75
WHERE StudentID = '2251161988' AND SubjectID = 'CSE204';

--Trigger kiểm tra điểm số hợp lệ trước khi chèn hoặc cập nhật:
CREATE TRIGGER CheckValidScore
ON Score
AFTER INSERT
AS
BEGIN
    IF EXISTS (SELECT * FROM inserted WHERE ContinuousAssessmentScore < 0 OR ContinuousAssessmentScore > 10 OR FinalExamScore < 0 OR FinalExamScore > 10)
    BEGIN
        PRINT'Scores must be between 0 and 10.'
        ROLLBACK TRANSACTION;
    END;
END;

-- Hồ Thủy Tiên
-- Trigger cập nhật Số tín, kì học, mô tả của các môn học
Drop table UpdateLog
CREATE TABLE UpdateLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    SubjectID VARCHAR(8),
    OldCredits INT,
    NewCredits INT,
    OldSemester INT,
    NewSemester INT,
    OldDescription NVARCHAR(255),
    NewDescription NVARCHAR(255),
	ChangDate DATETIME
);
GO

CREATE TRIGGER trgAfterUpdateSubject
ON [Subject]
AFTER UPDATE
AS
BEGIN

    IF EXISTS (SELECT 1 FROM [Subject] s JOIN inserted i ON s.SubjectID = i.SubjectID)
    BEGIN
 
        IF EXISTS (SELECT 1 FROM inserted WHERE Credits BETWEEN 1 AND 10
	AND Semester BETWEEN 1 AND 8 
	AND SubjectDescription IS NOT NULL 
	AND SubjectDescription <> '')
        BEGIN
            INSERT INTO UpdateLog (SubjectID, OldCredits, NewCredits, OldSemester, NewSemester, OldDescription,NewDescription,ChangDate)
            SELECT d.SubjectID, d.Credits, i.Credits, d.Semester, i.Semester, d.SubjectDescription, i.SubjectDescription, GETDATE()
            FROM
                deleted d
            JOIN
                inserted i ON d.SubjectID = i.SubjectID;
            UPDATE [Subject]
            SET
                Credits = i.Credits,
                Semester = i.Semester,
                SubjectDescription = i.SubjectDescription
            FROM
                inserted i
            WHERE
                [Subject].SubjectID = i.SubjectID;
        END
        ELSE
        BEGIN
            RAISERROR ('Dữ liệu không hợp lệ: Số tín chỉ phải nằm trong khoảng 1-10,
			kỳ học phải nằm trong khoảng 1-8 và mô tả môn học không được để trống.', 16, 1);
            ROLLBACK TRAN;
        END
    END
    ELSE
    BEGIN
        RAISERROR ('Môn học này không tồn tại, không thể cập nhật.', 16, 1);
        ROLLBACK TRAN;
    END
END;
GO

Select * from [Subject]
UPDATE [Subject]
SET
    Credits = 3,
    Semester = 2,
    SubjectDescription = N'Hiểu sâu hơn về cấu trúc dữ liệu và giải thuật với ngôn ngữ C++'
WHERE
    SubjectID = 'CSE281';

Select * from UpdateLog

-- Trigger Cập nhật số lượng sinh viên trong lớp khi thêm sinh viên
CREATE TRIGGER UpdateClassSize
ON Student
AFTER INSERT
AS
BEGIN
    -- Cập nhật số lượng sinh viên trong lớp khi có sinh viên mới được thêm
    UPDATE Class
    SET ClassSize = ClassSize + 1
    FROM Class c
    INNER JOIN INSERTED i ON c.ClassID = i.ClassID;
END;

-- Khổng Thị Vân
-- cập nhật trạng thái và điểm chữ của sinh viên trong bảng Score dựa trên tổng điểm.

CREATE TRIGGER trg_UpdateScoreStatus
ON Score
AFTER UPDATE
AS
BEGIN
    UPDATE Score
    SET Score.TotalScore = Score.ContinuousAssessmentScore * 0.5 + Score.FinalExamScore * 0.5
    FROM inserted i
    WHERE Score.StudentID = i.StudentID AND Score.SubjectID = i.SubjectID;

    UPDATE Score
    SET Score.LetterGrade = CASE
        WHEN Score.TotalScore >= 8.5 THEN 'A'
        WHEN Score.TotalScore >= 7.0 THEN 'B'
        WHEN Score.TotalScore >= 5.5 THEN 'C'
        WHEN Score.TotalScore >= 4.0 THEN 'D'
        ELSE 'F'
    END
    FROM inserted i
    WHERE Score.StudentID = i.StudentID AND Score.SubjectID = i.SubjectID;

    UPDATE Score
    SET Score.[Status] = CASE
        WHEN Score.TotalScore >= 4.0 THEN 'Pass'
        ELSE 'Fail'
    END
    FROM inserted i
    WHERE Score.StudentID = i.StudentID AND Score.SubjectID = i.SubjectID;
END;
GO

UPDATE Score
SET FinalExamScore = 8.0 
WHERE StudentID = '2251161988' AND SubjectID = 'CSE204';

select * from Score where StudentID = '2251161988' and [SubjectID] = 'CSE204';

-- Trigger Tự động cập nhật trạng thái điểm
CREATE TRIGGER UpdateScoreStatus
ON Score
AFTER UPDATE
AS
BEGIN
    UPDATE Score
    SET [Status] = CASE
        WHEN inserted.TotalScore >= 5 THEN 'Passed'
        ELSE 'Failed'
    END
    FROM inserted
    WHERE Score.StudentID = inserted.StudentID AND Score.SubjectID = inserted.SubjectID;
END

-- Vũ Hà Lâm
-- Thêm sinh viên mới

CREATE TABLE StudentInsertHistory (
    HistoryID INT IDENTITY(1,1) PRIMARY KEY,  -- Mã định danh tự động
    StudentID VARCHAR(10),
    StudentName NVARCHAR(100),
    ClassID VARCHAR(8),
    DepartmentID VARCHAR(8),
    DateOfBirth DATE,
    Gender NVARCHAR(10),
    Hometown NVARCHAR(100)
);

CREATE TRIGGER StudentInsert
ON Student
INSTEAD OF INSERT
AS
BEGIN
    -- Kiểm tra xem có mã sinh viên nào không đủ 10 ký tự số
    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE LEN(StudentID) <> 10 OR StudentID NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
    )
    BEGIN
        -- Ném lỗi nếu mã sinh viên không hợp lệ
        THROW 50001, N'Mã sinh viên phải đủ 10 số. Vui lòng sử dụng mã hợp lệ.', 1;
    END
    
    -- Kiểm tra xem có mã sinh viên nào đã tồn tại không
    IF EXISTS (SELECT 1 FROM Student WHERE StudentID IN (SELECT StudentID FROM inserted))
    BEGIN
        -- Ném lỗi nếu mã sinh viên đã tồn tại
        THROW 50002, N'Mã sinh viên đã tồn tại. Vui lòng sử dụng mã khác.', 1;
    END
    -- Nếu không có mã sinh viên nào trùng và mã hợp lệ, cho phép thêm bản ghi mới
	INSERT INTO Student (StudentID, StudentName, ClassID, DepartmentID, DateOfBirth, Gender, Hometown)
    SELECT StudentID, 
         StudentName, 
         ClassID, 
         DepartmentID, 
         DateOfBirth, 
         Gender, 
         Hometown
    FROM inserted;
	INSERT INTO StudentInsertHistory (StudentID, StudentName, ClassID, DepartmentID, DateOfBirth, Gender, Hometown)
    SELECT StudentID, 
        StudentName, 
        ClassID, 
        DepartmentID, 
        DateOfBirth, 
        Gender, 
        Hometown
    FROM inserted;
END;

--Nếu mã sinh viên không đủ 10 số
INSERT INTO Student (StudentID, StudentName, ClassID, DepartmentID, DateOfBirth, Gender, Hometown) 
VALUES
('225116', N'Nguyễn Văn A', '64CX2', 'CX', '2002-05-19', N'Nam', N'Hà Nội');
-- Nếu mã sinh viên đã tồn tại
INSERT INTO Student (StudentID, StudentName, ClassID, DepartmentID, DateOfBirth, Gender, Hometown) 
VALUES
('2251162064', N'Nguyễn Văn Hùng', '64CX2', 'CX', '2002-02-19', N'Nam', N'Hà Nội');

-- Nễu mã sinh viên chưa tồn tại
INSERT INTO Student (StudentID, StudentName, ClassID, DepartmentID, DateOfBirth, Gender, Hometown) 
VALUES
('2251162070', N'Ngô Thị Hoa', '64CX2', 'CX', '2003-10-26', N'Nữ ', N'Hà Nam')

select * from Student where ClassID= '64CX2'
select * from StudentInsertHistory

--Trigger Cập nhật số lượng sinh viên trong lớp khi sinh viên bị xóa

CREATE TRIGGER UpdateClassSizeOnStudentDelete
ON Student
AFTER DELETE
AS
BEGIN
    -- Cập nhật số lượng sinh viên trong lớp khi sinh viên bị xóa
    UPDATE Class
    SET ClassSize = ClassSize - 1
    WHERE ClassID = (SELECT ClassID FROM DELETED);
END;

-- Hoàng Khánh Hà
-- Thêm lớp mới

CREATE TABLE InsertClass (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    ClassID VARCHAR(8),
    ClassName NVARCHAR(100),
    ClassSize INT,
    DepartmentID VARCHAR(8),
    ChangeDate DATETIME
);
GO
CREATE TRIGGER trgBeforeInsertClass
ON Class
INSTEAD OF INSERT
AS
BEGIN
    -- Kiểm tra nếu lớp học đã tồn tại
    IF EXISTS (SELECT 1 FROM Class c JOIN inserted i ON c.ClassID = i.ClassID)
    BEGIN
        RAISERROR ('Lớp học này đã tồn tại, không thể thêm mới.', 16, 1);
        ROLLBACK TRAN;
    END
    ELSE
    BEGIN
        -- Nếu lớp học chưa tồn tại, thực hiện chèn dữ liệu mới
        INSERT INTO Class (ClassID, ClassName, ClassSize, DepartmentID)
        SELECT ClassID, ClassName, ClassSize, DepartmentID
        FROM inserted;
        
        -- Ghi lại thông tin vào bảng ClassLog
        INSERT INTO InsertClass(
            ClassID,
            ClassName,
            ClassSize,
            DepartmentID,
            ChangeDate
        )
        SELECT
            i.ClassID,
            i.ClassName,
            i.ClassSize,
            i.DepartmentID,
			GETDATE()
        FROM
            inserted i;
    END
END;
GO

INSERT INTO Class(ClassID, ClassName, ClassSize, DepartmentID)
VALUES ('65HTTT1', N'Khóa 65 Hệ Thống Thông Tin 1', 60,'CSE')

Select * from Class
Select * from InsertClass

--Trigger ngăn chặn việc xóa Department nếu Department đó còn có Class:
CREATE TRIGGER PreventDepartmentDeletion
ON Department
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT * FROM Class WHERE DepartmentID IN (SELECT DepartmentID FROM deleted))
    BEGIN
        PRINT 'Cannot delete department with existing classes.'
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        DELETE FROM Department WHERE DepartmentID IN (SELECT DepartmentID FROM deleted);
    END;
END;



-------------------------------------------------------------BỔ SUNG--------------------------------------------------------



3. FUNCTION


Function 1: Tính tổng học phí của sinh viên

CREATE FUNCTION TotalTuition (@StudentID VARCHAR(10))
RETURNS FLOAT
AS
BEGIN
    DECLARE @Total FLOAT;
    SELECT @Total = SUM(TuitionAmount)
    FROM TuitionFee
    WHERE StudentID = @StudentID;
    RETURN @Total;
END;



Function 2: Kiểm tra sinh viên có đỗ hay không

CREATE FUNCTION IsStudentPassed (@StudentID VARCHAR(10))
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT;
    IF EXISTS (SELECT * FROM Score WHERE StudentID = @StudentID AND TotalScore < 5)
        SET @Result = 0;
    ELSE
        SET @Result = 1;
    RETURN @Result;
END;


Function 3: Lấy số lượng sinh viên trong lớp
sql
Sao chép mã
CREATE FUNCTION GetStudentCount (@ClassID VARCHAR(8))
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(StudentID)
    FROM Student
    WHERE ClassID = @ClassID;
    RETURN @Count;
END;


Function 4: Tính tổng số tín chỉ sinh viên đã học

CREATE FUNCTION TotalCredits (@StudentID VARCHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @Credits INT;
    SELECT @Credits = SUM(Credits)
    FROM [Subject] SB
    JOIN Score SC ON SB.SubjectID = SC.SubjectID
    WHERE SC.StudentID = @StudentID;
    RETURN @Credits;
END;


Function 5: 

4. PROCEDURE
Procedure 1: Lấy danh sách sinh viên theo khoa

CREATE PROCEDURE GetStudentsByDepartment (@DepartmentID VARCHAR(8))
AS
BEGIN
    SELECT StudentID, StudentName
    FROM Student
    WHERE DepartmentID = @DepartmentID;
END;



Procedure 2: Thêm sinh viên mới

CREATE PROCEDURE AddNewStudent (
    @StudentID VARCHAR(10),
    @StudentName NVARCHAR(100),
    @ClassID VARCHAR(8),
    @DepartmentID VARCHAR(8),
    @DateOfBirth DATE,
    @Gender NVARCHAR(10),
    @Hometown NVARCHAR(100)
)
AS
BEGIN
    INSERT INTO Student (StudentID, StudentName, ClassID, DepartmentID, DateOfBirth, Gender, Hometown)
    VALUES (@StudentID, @StudentName, @ClassID, @DepartmentID, @DateOfBirth, @Gender, @Hometown);
END;

Procedure 3: Xóa sinh viên

CREATE PROCEDURE DeleteStudent (@StudentID VARCHAR(10))
AS
BEGIN
    DELETE FROM Student
    WHERE StudentID = @StudentID;
END; 


Proc 4: VIẾT 1 PROC TRẢ VỀ Danh sách SV THI TRƯỢT MÔN  CNPM, GỌI THỦ TỤC ĐÓ ĐỂ IN RA HỌ TÊN
alter PROCEDURE GetFailedStudentsInDBCourse
AS
BEGIN
    SELECT 
        S.StudentID, 
        S.StudentName
    FROM 
        Student S
        JOIN Score SC ON S.StudentID = SC.StudentID
        JOIN Subject SJ ON SC.SubjectID = SJ.SubjectID
    WHERE 
        SJ.SubjectName = N'Lập trình Python' AND SC.[Status] = 'Fail';
END;

-- Gọi thủ tục để in ra danh sách
EXEC GetFailedStudentsInDBCourse;


Proc 5: trả về danh sách sinh viên kèm thông tin môn học mà họ chưa đạt yêu cầu (điểm < 4.0)

CREATE PROCEDURE GetStudentsWithFailedSubjects
AS
BEGIN
    SELECT 
        S.StudentID,
        S.StudentName,
        SJ.SubjectName,
        SC.TotalScore
    FROM 
        Student S
        JOIN Score SC ON S.StudentID = SC.StudentID
        JOIN Subject SJ ON SC.SubjectID = SJ.SubjectID
    WHERE 
        SC.TotalScore < 4.0;
END;

-- Gọi thủ tục để lấy danh sách sinh viên thi trượt
EXEC GetStudentsWithFailedSubjects;



--------------CURSOR----------------------------------------
--Nguyễn Thị Thu Hằng
--Hiển thị danh sách sinh viên trong mỗi lớp
DECLARE cur CURSOR FOR 
SELECT ClassID FROM Class;

OPEN cur;

DECLARE @ClassID VARCHAR(8);
FETCH NEXT FROM cur INTO @ClassID;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT StudentID, StudentName FROM Student WHERE ClassID = @ClassID;
    FETCH NEXT FROM cur INTO @ClassID;
END;

CLOSE cur;
DEALLOCATE cur;

--Hồ Thủy Tiên
--Hiển thị danh sách sinh viên và điểm của họ trong một môn học
DECLARE cur CURSOR FOR 
SELECT StudentID, SubjectID FROM Score;

OPEN cur;

DECLARE @StudentID VARCHAR(10), @SubjectID VARCHAR(8);
FETCH NEXT FROM cur INTO @StudentID, @SubjectID;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT S.StudentName, SB.SubjectName, SC.TotalScore
    FROM Score SC
    JOIN Student S ON SC.StudentID = S.StudentID
    JOIN [Subject] SB ON SC.SubjectID = SB.SubjectID
    WHERE SC.StudentID = @StudentID AND SC.SubjectID = @SubjectID;
    
    FETCH NEXT FROM cur INTO @StudentID, @SubjectID;
END;

CLOSE cur;
DEALLOCATE cur;


--Khổng Thị Vân
--Cập nhật học phí cho sinh viên theo kỳ
DECLARE cur CURSOR FOR 
SELECT StudentID, Semester, TuitionAmount FROM TuitionFee;

OPEN cur;

DECLARE @StudentID VARCHAR(10), @Semester INT, @TuitionAmount FLOAT;
FETCH NEXT FROM cur INTO @StudentID, @Semester, @TuitionAmount;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @Semester = 1
    BEGIN
        UPDATE TuitionFee
        SET TuitionAmount = @TuitionAmount * 1.1
        WHERE StudentID = @StudentID AND Semester = @Semester;
    END
    ELSE
    BEGIN
        UPDATE TuitionFee
        SET TuitionAmount = @TuitionAmount * 1.05
        WHERE StudentID = @StudentID AND Semester = @Semester;
    END
    
    FETCH NEXT FROM cur INTO @StudentID, @Semester, @TuitionAmount;
END;

CLOSE cur;
DEALLOCATE cur;


--Vũ Hà Lâm
-- Lấy thông tin về các sinh viên đã nhận thưởng
DECLARE cur CURSOR FOR 
SELECT StudentID FROM Award;

OPEN cur;

DECLARE @StudentID VARCHAR(10);
FETCH NEXT FROM cur INTO @StudentID;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT S.StudentName, A.AwardAmount, A.Reason
    FROM Award A
    JOIN Student S ON A.StudentID = S.StudentID
    WHERE A.StudentID = @StudentID;
    
    FETCH NEXT FROM cur INTO @StudentID;
END;

CLOSE cur;
DEALLOCATE cur;


--Hoàng Khánh Hà
--Cập nhật trạng thái học bổng cho sinh viên
DECLARE cur CURSOR FOR 
SELECT StudentID, AwardAmount FROM Award;

OPEN cur;

DECLARE @StudentID VARCHAR(10), @AwardAmount NVARCHAR(50);
FETCH NEXT FROM cur INTO @StudentID, @AwardAmount;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @AwardAmount = 'High' 
    BEGIN
        UPDATE Student
        SET Hometown = 'Top Scholar'
        WHERE StudentID = @StudentID;
    END
    
    FETCH NEXT FROM cur INTO @StudentID, @AwardAmount;
END;

CLOSE cur;
DEALLOCATE cur;




----------VIEW-----------------------


--------------TRIGGER---------------
